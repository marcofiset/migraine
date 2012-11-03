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
        [Test]
        public void TestNumber()
        {
            var tokenQueue = new Queue<Token>();
            tokenQueue.Enqueue(new Token("32", TokenType.Number));

            var parser = new Parser(tokenQueue);
            var expression = parser.Parse();

            Assert.AreEqual(32, expression.Evaluate());
        }

        [Test]
        public void TestNegativeNumber()
        {
            var tokenQueue = new Queue<Token>();
            tokenQueue.Enqueue(new Token("-", TokenType.Operator));
            tokenQueue.Enqueue(new Token("32", TokenType.Number));

            var parser = new Parser(tokenQueue);
            var expression = parser.Parse();

            Assert.AreEqual(-32, expression.Evaluate());
        }

        private Node GetExpressionFromNumbersAndOperator(double n1, string op, double n2)
        {
            var tokenQueue = new Queue<Token>();

            tokenQueue.Enqueue(new Token(n1.ToString(), TokenType.Number));
            tokenQueue.Enqueue(new Token(op, TokenType.Operator));
            tokenQueue.Enqueue(new Token(n2.ToString(), TokenType.Number));

            var parser = new Parser(tokenQueue);

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
    }
}
