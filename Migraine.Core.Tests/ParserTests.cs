using Migraine.Core.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private TokenStream _tokenStream;

        [SetUp]
        public void SetUp()
        {
            _tokenStream = new TokenStream();
        }

        [Test]
        public void TestNumber()
        {
            _tokenStream.Add(new Token("32", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(32, expression.Evaluate());
        }

        [Test]
        public void TestNegativeNumber()
        {
            _tokenStream.Add(new Token("-", TokenType.Operator));
            _tokenStream.Add(new Token("32", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(-32, expression.Evaluate());
        }

        private Node GetExpressionFromNumbersAndOperator(double n1, string op, double n2)
        {
            _tokenStream.Add(new Token(n1.ToString(), TokenType.Number));
            _tokenStream.Add(new Token(op, TokenType.Operator));
            _tokenStream.Add(new Token(n2.ToString(), TokenType.Number));

            var parser = new Parser(_tokenStream);

            return parser.Parse();
        }


        [Test]
        public void TestAddition()
        {
            var expression = GetExpressionFromNumbersAndOperator(32, "+", 40);

            Assert.AreEqual(72, expression.Evaluate());
        }

        [Test]
        public void TestSubstraction()
        {
            var expression = GetExpressionFromNumbersAndOperator(42, "-", 30);

            Assert.AreEqual(12, expression.Evaluate());
        }

        [Test]
        public void TestMultiplication()
        {
            var expression = GetExpressionFromNumbersAndOperator(8, "*", 7);

            Assert.AreEqual(56, expression.Evaluate());
        }

        [Test]
        public void TestDivision()
        {
            var expression = GetExpressionFromNumbersAndOperator(56, "/", 7);

            Assert.AreEqual(8, expression.Evaluate());
        }

        [Test]
        public void TestMultipleAddition()
        {
            _tokenStream.Add(new Token("15", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("20", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("5", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(40, expression.Evaluate());
        }

        [Test]
        public void TestOperatorPrecedence()
        {
            _tokenStream.Add(new Token("15", TokenType.Number));
            _tokenStream.Add(new Token("-", TokenType.Operator));
            _tokenStream.Add(new Token("20", TokenType.Number));
            _tokenStream.Add(new Token("/", TokenType.Operator));
            _tokenStream.Add(new Token("5", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("10", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(21, expression.Evaluate());
        }

        [Test]
        public void TestMultipleMultiplication()
        {
            _tokenStream.Add(new Token("5", TokenType.Number));
            _tokenStream.Add(new Token("*", TokenType.Operator));
            _tokenStream.Add(new Token("2", TokenType.Number));
            _tokenStream.Add(new Token("*", TokenType.Operator));
            _tokenStream.Add(new Token("8", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(80, expression.Evaluate());
        }

        [Test]
        public void TestMultipleDivision()
        {
            _tokenStream.Add(new Token("80", TokenType.Number));
            _tokenStream.Add(new Token("/", TokenType.Operator));
            _tokenStream.Add(new Token("20", TokenType.Number));
            _tokenStream.Add(new Token("/", TokenType.Operator));
            _tokenStream.Add(new Token("2", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(2, expression.Evaluate());
        }

        [Test]
        public void TestVeryComplexExpression()
        {
            _tokenStream.Add(new Token("2", TokenType.Number));
            _tokenStream.Add(new Token("*", TokenType.Operator));
            _tokenStream.Add(new Token("8", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("21", TokenType.Number));
            _tokenStream.Add(new Token("/", TokenType.Operator));
            _tokenStream.Add(new Token("7", TokenType.Number));
            _tokenStream.Add(new Token("*", TokenType.Operator));
            _tokenStream.Add(new Token("4", TokenType.Number));
            _tokenStream.Add(new Token("-", TokenType.Operator));
            _tokenStream.Add(new Token("15", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("6", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("3", TokenType.Number));
            _tokenStream.Add(new Token("*", TokenType.Operator));
            _tokenStream.Add(new Token("8", TokenType.Number));
            _tokenStream.Add(new Token("/", TokenType.Operator));
            _tokenStream.Add(new Token("4", TokenType.Number));
            _tokenStream.Add(new Token("/", TokenType.Operator));
            _tokenStream.Add(new Token("2", TokenType.Number));
            _tokenStream.Add(new Token("-", TokenType.Operator));
            _tokenStream.Add(new Token("1", TokenType.Number));
            _tokenStream.Add(new Token("+", TokenType.Operator));
            _tokenStream.Add(new Token("4", TokenType.Number));

            var parser = new Parser(_tokenStream);
            var expression = parser.Parse();

            Assert.AreEqual(25, expression.Evaluate());
        }

        [Test]
        public void TestSimpleParenthesisExpression()
        {
            var _tokenStream = new MigraineLexer().Tokenize("(3)");
            var expression = new Parser(_tokenStream).Parse();

            Assert.AreEqual(3, expression.Evaluate());
        }

        [Test]
        public void TestParenthesisPrecedence()
        {
            var tokens = new MigraineLexer().Tokenize("(3 + 4) * 2");
            var expression = new Parser(tokens).Parse();

            Assert.AreEqual(14, expression.Evaluate());
        }

        [Test]
        public void TestUnaryMinus()
        {
            var tokens = new MigraineLexer().Tokenize("-(-3 + 5 * -(14 - -7)) / -2");
            var expression = new Parser(tokens).Parse();

            Assert.AreEqual(-54, expression.Evaluate());
        }
    }
}
