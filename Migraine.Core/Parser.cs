using Migraine.Core.Nodes;
using Migraine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core
{
    /// <summary>
    /// This is the parser class.
    /// The following grammar is supported :
    /// 
    /// Expression :== Term ( ('+' | '-') Term)*
    /// Term       :== Factor ( ('*' | '/') Factor )*
    /// Factor     :== ('-')? ( Number | ParenExpression )
    /// ParenExpression :== '(' Expression ')'
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

        public Parser(TokenStream tokens)
        {
            tokenStream = tokens;
        }

        public Node Parse()
        {
            return ParseExpression();
        }

        private Node ParseExpression()
        {
            var leftTerm = ParseTerm();
            if (tokenStream.IsEmpty) return leftTerm;

            var restOfExpression = new List<Tuple<string, Node>>();

            while (CurrentToken.Value == "+" || CurrentToken.Value == "-")
            {
                var op = CurrentToken.Value;
                tokenStream.Consume();

                var rightTerm = ParseTerm();

                restOfExpression.Add(Tuple.Create(op, rightTerm));

                if (tokenStream.IsEmpty) break;
            }

            if (restOfExpression.Count == 0)
                return leftTerm;

            return new OperationNode(leftTerm, restOfExpression);
        }

        private Node ParseTerm()
        {
            var leftFactor = ParseFactor();
            if (tokenStream.IsEmpty) return leftFactor;

            var restOfExpression = new List<Tuple<string, Node>>();

            while (CurrentToken.Value == "*" || CurrentToken.Value == "/")
            {
                var op = CurrentToken.Value;
                tokenStream.Consume();

                var rightFactor = ParseFactor();

                restOfExpression.Add(Tuple.Create(op, rightFactor));

                if (tokenStream.IsEmpty) break;
            }

            if (restOfExpression.Count == 0)
                return leftFactor;

            return new OperationNode(leftFactor, restOfExpression); ;
        }

        private Node ParseFactor()
        {
            bool positive = true;

            if (CurrentToken.Value == "-")
            {
                tokenStream.Consume();
                positive = false;
            }

            Node factor = null;

            if (CurrentToken.Type == TokenType.Number)
            {
                Double termValue = Convert.ToDouble(CurrentToken.Value);
                tokenStream.Consume();

                factor = new NumberNode(termValue);
            }
            else if (CurrentToken.Value == "(")
            {
                tokenStream.Consume();
                factor = ParseExpression();

                tokenStream.Consume(")");
            }

            if (factor == null)
                throw new Exception("Number or () expression expected");

            if (!positive)
                return new UnaryMinusNode(factor);

            return factor;
        }
    }
}
