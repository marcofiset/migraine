using Migraine.Core.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Tests
{
    public class ParserTests
    {
        private Parser _parser;
        private TokenStream _tokens;

        [SetUp]
        public void SetUp()
        {
            _tokens = new TokenStream();
            _parser = new Parser(_tokens);
        }

        [Test]
        public void CanParseNumberNode()
        {
            _tokens.Add(new Token("32", TokenType.Number));

            var node = _parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<NumberNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseUnaryMinusNode()
        {
            _tokens.Add(new Token("-", TokenType.Operator));
            _tokens.Add(new Token("23.5", TokenType.Number));

            var node = _parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<UnaryMinusNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseOperationNode()
        {
            _tokens.Add(new Token("32", TokenType.Number));
            _tokens.Add(new Token("-", TokenType.Operator));
            _tokens.Add(new Token("24", TokenType.Number));

            var node = _parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<OperationNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseExpressionListNode()
        {
            _tokens.Add(new Token("32", TokenType.Number));
            _tokens.Add(new Token("-", TokenType.Operator));
            _tokens.Add(new Token("24", TokenType.Number));

            _tokens.Add(new Token(@"\n", TokenType.NewLine));

            _tokens.Add(new Token("8", TokenType.Number));
            _tokens.Add(new Token("*", TokenType.Operator));
            _tokens.Add(new Token("9", TokenType.Number));

            var node = _parser.Parse();

            Assert.IsInstanceOf<ExpressionListNode>(node);
            Assert.AreEqual(2, (node as ExpressionListNode).Expressions.Count());
        }
    }
}
