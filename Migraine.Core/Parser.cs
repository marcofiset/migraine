using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Migraine.Core.Nodes;

namespace Migraine.Core
{
    /// <summary>
    /// This is the parser class.
    /// The following grammar is supported :
    /// 
    /// ExpressionList     = { Expression, { Terminator } } //Terminator when following Assignment, Operation or FunctionCall
    /// Expression         = Assignment | Operation | Block | FunctionDefinition | IfStatement
    /// Assignment         = Identifier, "=", Expression
    /// Operation          = Term { "+" | "-", Term }
    /// Term               = Factor { "*" | "/", Factor }
    /// Factor             = [ "-" ], Number | Identifier | FunctionCall | ParenExpression
    /// ParenExpression    = "(", Expression, ")"
    /// FunctionDefinition = "fun", Identifier, "(", IdentifierList, ")", Block
    /// IfStatement        = "if", "(", Condition, ")", Block
    /// Condition          = Expression, [ ComparisonOperator, Expression ]
    /// ComparisonOperator = "==" | "<=" | ">=" | "<" | ">"
    /// Block              = "{", ExpressionList, "}"
    /// FunctionCall       = Identifier, "(", ArgumentList, ")"
    /// IdentifierList     = Identifier, { ",", Identifier }
    /// ArgumentList       = Expression, { ",", Expression }
    /// Terminator         = ";"
    /// </summary>
    public class Parser
    {
        private TokenStream tokenStream;

        private Token CurrentToken
        {
            get { return tokenStream.CurrentToken; }
        }
        
        private Token ConsumedToken
        {
            get { return tokenStream.ConsumedToken; }
        }

        public Parser(TokenStream tokens)
        {
            tokenStream = tokens;
        }

        public Node Parse()
        {
            return ParseExpressionList();
        }

        private Node ParseExpressionList()
        {
            var result = new List<Node>();

            while (!tokenStream.IsEmpty)
            {
                result.Add(ParseExpression());

                if (!tokenStream.IsEmpty)
                {
                    var nodeType = result.Last().GetType();

                    if (nodeType == typeof(AssignmentNode) || nodeType == typeof(OperationNode) || nodeType == typeof(FunctionCallNode))
                        tokenStream.Expect(TokenType.Terminator);
                }
            }

            return new ExpressionListNode(result);
        }

        // Expression = Assignment | Operation | Block | FunctionDefinition | IfStatement
        private Node ParseExpression()
        {
            var currentToken = tokenStream.CurrentToken;

            if (currentToken.Type == TokenType.Identifier)
            {
                if (currentToken.Value == "fun")
                    return ParseFunctionDefinition();

                if (currentToken.Value == "if")
                    return ParseIfStatement();

                var lookAheadToken = tokenStream.LookAhead();

                if (lookAheadToken != null)
                {
                    if (lookAheadToken.Value == "=")
                        return ParseAssignment();
                }
            }

            if (currentToken.Value == "{")
                return ParseBlock();

            return ParseOperation();
        }

        // FunctionDefinition = "fun", Identifier, "(", IdentifierList, ")", Block
        private FunctionDefinitionNode ParseFunctionDefinition()
        {
            tokenStream.Expect("fun");
            tokenStream.Expect(TokenType.Identifier);

            var name = ConsumedToken.Value;
            tokenStream.Expect("(");

            List<String> arguments;

            if (tokenStream.Consume(")"))
                arguments = new List<String>();
            else
                arguments = ParseIdentifierList();

            tokenStream.Expect(")");

            var body = ParseBlock();

            return new FunctionDefinitionNode(name, arguments, body);
        }

        // IdentifierList = Identifier, { ",", Identifier }
        private List<String> ParseIdentifierList()
        {
            var arguments = new List<String>();

            while (tokenStream.Consume(TokenType.Identifier))
            {
                arguments.Add(ConsumedToken.Value);
                tokenStream.Consume(",");
            }

            return arguments;
        }

        // IfStatement = "if", "(", Condition, ")", Block
        private IfStatementNode ParseIfStatement()
        {
            tokenStream.Expect("if");
            tokenStream.Expect("(");

            var condition = ParseCondition();

            tokenStream.Expect(")");

            var body = ParseBlock();

            return new IfStatementNode(condition, body);
        }

        //Condition = Expression, [ ComparisonOperator, Expression ]
        private ConditionNode ParseCondition()
        {
            var left = ParseExpression();

            if (!tokenStream.Consume(TokenType.ComparisonOperator))
                return new ConditionNode(left);

            var op = ConsumedToken.Value;
            var right = ParseExpression();

            return new ConditionNode(left, op, right);
        }

        // Block = "{", ExpressionList, "}"
        private BlockNode ParseBlock()
        {
            tokenStream.Expect("{");

            if (tokenStream.Consume("}"))
                return new BlockNode();

            var expressions = new List<Node>();

            while (!tokenStream.Consume("}"))
            {
                expressions.Add(ParseExpression());
                tokenStream.Consume(TokenType.Terminator);
            }

            return new BlockNode(expressions);
        }

        // FunctionCall = Identifier, "(", ArgumentList, ")"
        private FunctionCallNode ParseFunctionCall()
        {
            tokenStream.Expect(TokenType.Identifier);
            var name = ConsumedToken.Value;

            tokenStream.Expect("(");

            if (tokenStream.Consume(")"))
                return new FunctionCallNode(name);
            
            var arguments = ParseArgumentList();
            tokenStream.Expect(")");

            return new FunctionCallNode(name, arguments);
        }

        // ArgumentList = Expression, { ",", Expression }
        private List<Node> ParseArgumentList()
        {
            var arguments = new List<Node>();
            
            do
            {
                arguments.Add(ParseExpression());
            } while (tokenStream.Consume(","));

            return arguments;
        }

        // Assignment = Identifier, "=", Expression
        private AssignmentNode ParseAssignment()
        {
            tokenStream.Consume(TokenType.Identifier);
            var identifier = ConsumedToken.Value;

            tokenStream.Expect("=");
            var expression = ParseExpression();

            return new AssignmentNode(identifier, expression);
        }

        // Operation = Term { "+" | "-", Term }
        private Node ParseOperation()
        {
            return ParseOperators(ParseTerm, "+", "-");
        }

        // Term = Factor { "*" | "/", Factor }
        private Node ParseTerm()
        {
            return ParseOperators(ParseFactor, "*", "/");
        }

        private Node ParseOperators(Func<Node> operandParser, params string[] operators)
        {
            var leftOperand = operandParser();
            if (tokenStream.IsEmpty) return leftOperand;

            var restOfExpression = new List<Tuple<string, Node>>();

            while (!tokenStream.IsEmpty && tokenStream.ConsumeAny(operators))
            {
                var op = ConsumedToken.Value;
                var rightOperand = operandParser();

                restOfExpression.Add(Tuple.Create(op, rightOperand));
            }

            if (restOfExpression.Count == 0)
                return leftOperand;

            return new OperationNode(leftOperand, restOfExpression);
        }

        // Factor = [ "-" ], Number | Identifier | ParenExpression | FunctionCall
        private Node ParseFactor()
        {
            if (tokenStream.Consume("-"))
                return new UnaryMinusNode(ParseFactor());

            Node factor = null;

            if (tokenStream.Consume(TokenType.Number))
            {
                Double termValue = Convert.ToDouble(ConsumedToken.Value, CultureInfo.InvariantCulture);

                factor = new NumberNode(termValue);
            }
            else if (CurrentToken.Type == TokenType.Identifier)
            {
                var lookAhead = tokenStream.LookAhead();
                if (lookAhead != null && lookAhead.Value == "(")
                    return ParseFunctionCall();

                tokenStream.Consume();
                factor = new IdentifierNode(tokenStream.ConsumedToken.Value);
            }
            else if (tokenStream.Consume("("))
            {
                factor = ParseExpression();

                tokenStream.Expect(")");
            }

            if (factor == null)
                throw new Exception("Number, identifier or function call expected");

            return factor;
        }
    }
}
