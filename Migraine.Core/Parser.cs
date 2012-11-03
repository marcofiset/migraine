using Migraine.Core.Nodes;
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
    /// Expression :== Term ( ('+' | '-') Term )?
    /// Term       :== Factor ( ('*' | '/') Factor )?
    /// Factor     :== ('-')? Number
    /// 
    /// What I would like to support in the future :
    /// 
    /// Expression :== Term ( ('+' | '-') Term)*
    /// Term       :== Factor ( ('*' | '/') Factor )*
    /// Factor     :== ('-')? ( Number | ParenExpression ) ('^' (Number | ParenExpression))?
    /// ParenExpression :== '(' Expression ')'
    /// 
    /// Number is defined by the Number token type provided by the Lexer
    /// 
    /// This is a grammar that I defined myself as I don't really like
    /// standard grammar specifications. I think this one is easier to follow.
    /// 
    /// It currently does not support exponent operator
    /// </summary>
    public class Parser
    {
        private Queue<Token> tokenQueue;

        public Parser(Queue<Token> tokens)
        {
            tokenQueue = tokens;
        }

        public Node Parse()
        {
            return ParseExpressionNode();
        }

        private Node ParseExpressionNode()
        {
            var leftTerm = ParseTerm();
            if (tokenQueue.Count == 0) return leftTerm;

            var currentToken = tokenQueue.Peek();

            if (currentToken.Value == "+" || currentToken.Value == "-")
            {
                var op = tokenQueue.Dequeue().Value;
                var rightTerm = ParseTerm();

                return new OperationNode(leftTerm, op, rightTerm);
            }

            throw new Exception("Operator expected (+ or -)");
        }

        private Node ParseTerm()
        {
            var leftFactor = ParseFactor();
            if (tokenQueue.Count == 0) return leftFactor;

            var currentToken = tokenQueue.Peek();

            if (currentToken.Value == "*" || currentToken.Value == "/")
            {
                var op = tokenQueue.Dequeue().Value;
                var rightFactor = ParseFactor();

                return new OperationNode(leftFactor, op, rightFactor);
            }

            return leftFactor;
        }

        private Node ParseFactor()
        {
            var currentToken = tokenQueue.Peek();
            bool positive = true;

            if (currentToken.Value == "-")
            {
                tokenQueue.Dequeue();
                positive = false;
                currentToken = tokenQueue.Peek();
            }

            if (currentToken.Type == TokenType.Number)
            {
                tokenQueue.Dequeue();
                Double termValue = Convert.ToDouble(currentToken.Value);
                if (!positive) termValue *= -1;

                return new NumberNode(termValue);
            }

            throw new Exception("Number expected");
        }
    }
}
