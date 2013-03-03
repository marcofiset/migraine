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
    public class IntegrationTests
    {
        private TokenStream _tokenStream;
        private MigraineLexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _tokenStream = new TokenStream();
            _lexer = new MigraineLexer();
        }

        private Double EvaluateExpression(String expression)
        {
            _tokenStream = _lexer.Tokenize(expression);
            var parser = new Parser(_tokenStream);

            return parser.Parse().Evaluate();
        }

        [Test]
        public void TestAddition()
        {
            Assert.AreEqual(72, EvaluateExpression("32 + 40"));
            Assert.AreEqual(40, EvaluateExpression("15 + 20 + 5"));
        }

        [Test]
        public void TestSubstraction()
        {
            Assert.AreEqual(12, EvaluateExpression("42 - 30"));
            Assert.AreEqual(6, EvaluateExpression("42 - 30 - 6"));
        }

        [Test]
        public void TestMultiplication()
        {
            Assert.AreEqual(56, EvaluateExpression("8 * 7"));
            Assert.AreEqual(80, EvaluateExpression("5 * 2 * 8"));
        }

        [Test]
        public void TestDivision()
        {
            Assert.AreEqual(8, EvaluateExpression("56 / 7"));
            Assert.AreEqual(2, EvaluateExpression("80 / 20 / 2"));
        }

        [Test]
        public void TestOperatorPrecedence()
        {
            Assert.AreEqual(31, EvaluateExpression("15 - 20 / 5 + 10 * 2"));
        }

        [Test]
        public void TestVeryComplexExpression()
        {
            Assert.AreEqual(25, EvaluateExpression("2 * 8 + 21 / 7 * 4 - 15 + 6 + 3 * 8 / 4 / 2 - 1 + 4"));
        }

        [Test]
        public void TestSimpleParenthesisExpression()
        {
            Assert.AreEqual(3, EvaluateExpression("(3)"));
        }

        [Test]
        public void TestParenthesisPrecedence()
        {
            Assert.AreEqual(14, EvaluateExpression("(3 + 4) * 2"));
            Assert.AreEqual(3.5, EvaluateExpression("(3 + 4) / 2"));
        }

        [Test]
        public void TestUnaryMinus()
        {
            Assert.AreEqual(-54, EvaluateExpression("-(-3 + 5 * -(14 - -7)) / -2"));
        }
    }
}