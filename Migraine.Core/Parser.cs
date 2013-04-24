using Migraine.Core.Nodes;
using Migraine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Migraine.Core
{
    /// <summary>
    /// This is the parser class.
    /// The following grammar is supported :
    /// 
    /// ExpressionList     = Expression { Terminator, Expression }, Terminator
    /// Expression         = Assignment | Operation | Block | FunctionCall | FunctionDefinition
    /// Assignment         = Identifier, "=", Expression
    /// Operation          = Term { "+" | "-", Term }
    /// Term               = Factor { "*" | "/", Factor }
    /// Factor             = [ "-" ], Number | Identifier | ParenExpression
    /// ParenExpression    = "(", Expression, ")"
    /// FunctionDefinition = "fun", Identifier, "(", IdentifierList, ")", Block
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

        // ExpressionList = { Expression, Terminator }
        private Node ParseExpressionList()
        {
            var result = new List<Node>();

            while (!tokenStream.IsEmpty)
            {
                result.Add(ParseExpression());

                // Force a terminator only if we have more than one expression
                if (!tokenStream.IsEmpty)
                    tokenStream.Expect(TokenType.Terminator);
            }

            return new ExpressionListNode(result);
        }

        // Expression = Assignment | Operation | Block | FunctionCall | FunctionDefinition
        private Node ParseExpression()
        {
            var currentToken = tokenStream.CurrentToken;

            if (currentToken.Type == TokenType.Identifier)
            {
                if (currentToken.Value == "fun")
                    return ParseFunctionDefinition();

                var lookAheadToken = tokenStream.LookAhead();

                if (lookAheadToken != null)
                {
                    if (lookAheadToken.Value == "=")
                        return ParseAssignment();

                    if (lookAheadToken.Value == "(")
                        return ParseFunctionCall();
                }
            }

            if (currentToken.Value == "{")
                return ParseBlock();

            return ParseOperation();
        }

        // FunctionDefinition = "fun", Identifier, "(", IdentifierList, ")", Block
        private Node ParseFunctionDefinition()
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

            var body = ParseBlock() as BlockNode;

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

        // Block = "{", ExpressionList, "}"
        private Node ParseBlock()
        {
            tokenStream.Expect("{");

            if (tokenStream.Consume("}"))
                return new BlockNode();

            var expressionList = ParseExpressionList() as ExpressionListNode;

            tokenStream.Expect("}");

            return new BlockNode(expressionList.Expressions);
        }

        // FunctionCall = Identifier, "(", ArgumentList, ")"
        private Node ParseFunctionCall()
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
        private Node ParseAssignment()
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
            var leftTerm = ParseTerm();
            if (tokenStream.IsEmpty) return leftTerm;

            var restOfExpression = new List<Tuple<string, Node>>();

            while (!tokenStream.IsEmpty && tokenStream.ConsumeAny("+", "-"))
            {
                var op = ConsumedToken.Value;
                var rightTerm = ParseTerm();

                restOfExpression.Add(Tuple.Create(op, rightTerm));
            }

            if (restOfExpression.Count == 0)
                return leftTerm;

            return new OperationNode(leftTerm, restOfExpression);
        }

        // Term = Factor { "*" | "/", Factor }
        private Node ParseTerm()
        {
            var leftFactor = ParseFactor();
            if (tokenStream.IsEmpty) return leftFactor;

            var restOfExpression = new List<Tuple<string, Node>>();

            while (!tokenStream.IsEmpty && tokenStream.ConsumeAny("*", "/"))
            {
                var op = ConsumedToken.Value;
                var rightFactor = ParseFactor();

                restOfExpression.Add(Tuple.Create(op, rightFactor));
            }

            if (restOfExpression.Count == 0)
                return leftFactor;

            return new OperationNode(leftFactor, restOfExpression);
        }

        // Factor = [ "-" ], Number | Identifier | ParenExpression
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
            else if (tokenStream.Consume(TokenType.Identifier))
            {
                factor = new IdentifierNode(tokenStream.ConsumedToken.Value);
            }
            else if (tokenStream.Consume("("))
            {
                factor = ParseExpression();

                tokenStream.Expect(")");
            }

            if (factor == null)
                throw new Exception("Number, identifier or () expression expected");

            return factor;
        }
    }
}
