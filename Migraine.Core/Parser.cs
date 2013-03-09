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
    /// ExpressionList  = Expression { Terminator, Expression }
    /// Expression      = Assignment | Operation
    /// Assignment      = Identifier, "=", Expression
    /// Operation       = Term { "+" | "-", Term }
    /// Term            = Factor { "*" | "/", Factor }
    /// Factor          = [ "-" ], Number | Identifier | ParenExpression
    /// ParenExpression = "(", Expression, ")"
    /// Terminator      = NewLine
    /// 
    /// Number is defined by the Number token type provided by the Lexer
    /// 
    /// I decided not to support the exponent operator because of precedence issue that might arise,
    /// as there are no universally accepted way of doing it right.
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

        // ExpressionList = Expression { Terminator, Expression }
        private Node ParseExpressionList()
        {
            var result = new List<Node>();

            result.Add(ParseExpression());

            while (!tokenStream.IsEmpty)
            {
                tokenStream.Expect(TokenType.Terminator);
                result.Add(ParseExpression());
            }

            return new ExpressionListNode(result);
        }

        // Expression = Assignment | Operation
        private Node ParseExpression()
        {
            var token = tokenStream.LookAhead();

            if (token != null && token.Value == "=")
                return ParseAssignment();

            return ParseOperation();
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

        // Assignment = Identifier, "=", Expression
        private Node ParseAssignment()
        {
            tokenStream.Consume(TokenType.Identifier);
            var identifier = ConsumedToken.Value;

            tokenStream.Expect("=");
            var expression = ParseExpression();

            return new AssignmentNode(identifier, expression);
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

            return new OperationNode(leftFactor, restOfExpression); ;
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
