using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using ExpressionEvaluator;

namespace ExpressionEvalutor.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        private ExpressionEvaluator.ExpressionEvaluator evaluator;

        [SetUp]
        public void SetUp()
        {
            evaluator = new ExpressionEvaluator.ExpressionEvaluator();
        }

        [Test]
        public void CanUseAddAsInfixOperator()
        {
            Assert.That(evaluator.Evaluate("45 + 2") == 47);
            Assert.That(evaluator.Evaluate("10 + 25 + 3 + 1") == 39);
        }

        [Test]
        public void CanUseMinusAsInfixOperator()
        {
            Assert.That(evaluator.Evaluate("25 - 18") == 7);
            Assert.That(evaluator.Evaluate("50 - 25 - 13") == 12);
        }

        [Test]
        public void CanUseMultiplyOperator()
        {
            Assert.That(evaluator.Evaluate("4 * 12") == 48);
            Assert.That(evaluator.Evaluate("3 * 8 * 10") == 240);
        }

        [Test]
        public void CanUseDivideOperator()
        {
            Assert.That(evaluator.Evaluate("18 / 6") == 3);
            Assert.That(evaluator.Evaluate("48 / 8 / 3") == 2);
        }

        [Test]
        public void MultiplyAndDivideHaveGreaterPrecedenceThanAddAndMinus()
        {
            Assert.That(evaluator.Evaluate("4 + 8 * 12") == 100);
            Assert.That(evaluator.Evaluate("4 + 8 / 2") == 8);
            Assert.That(evaluator.Evaluate("32 - 6 * 2") == 20);
            Assert.That(evaluator.Evaluate("8 + 4 / 2") == 10);
        }

        [Test]
        public void CanUseExponentOperator()
        {
            Assert.That(evaluator.Evaluate("2 ^ 4") == 16);
        }

        [Test]
        public void ExponentOperatorIsRightAssociative()
        {
            Assert.That(evaluator.Evaluate("4 ^ 2 ^ 3") == Math.Pow(4, 8));
        }

        [Test]
        public void ExponentOperatorHasHigherPrecedenceThanPlusMinusMultiplyDivide()
        {
            Assert.That(evaluator.Evaluate("3 * 2 ^ 2") == 12);
            Assert.That(evaluator.Evaluate("24 / 2 ^ 3") == 3);
            Assert.That(evaluator.Evaluate("3 + 2 ^ 2") == 7);
            Assert.That(evaluator.Evaluate("24 - 2 ^ 3") == 16);
        }

        [Test]
        public void CanUseUnaryMinusOperator()
        {
            Assert.That(evaluator.Evaluate("-3 + 14") == 11);
            Assert.That(evaluator.Evaluate("3 + -14") == -11);
            Assert.That(evaluator.Evaluate("3 - -14") == 17);
            Assert.That(evaluator.Evaluate("16 * -4") == -64);
            Assert.That(evaluator.Evaluate("-32 / -2") == 16);
        }

        [Test]
        public void UnaryMinusHasGreatestPrecedence()
        {
            Assert.That(evaluator.Evaluate("-2^3") == -8);
            Assert.That(evaluator.Evaluate("-2^2") == 4);
            Assert.That(evaluator.Evaluate("2^-2") == 0.25);
        }

        [Test]
        public void ParenthesisAreEvaluatedFirst()
        {
			Assert.That(evaluator.Evaluate("(2 + 3) * 10") == 50);
        }

        [Test]
        public void CanUseMinusAsPrefixOperatorToParenthesis()
        {
			Assert.That(evaluator.Evaluate("-(2 + 3) * 10") == -50);
        }

        [Test]
        public void MismatchedParenthesisShouldThrowParsingException()
        {
            Assert.Throws(typeof(Exception), () => evaluator.Evaluate("4 + 2) * 3"));
        }

        [Test]
        public void UnexpectedTokenShouldThrowException()
        {
            Assert.Throws(typeof(Exception), () => evaluator.Evaluate("4 & 4 + 2"));
        }
    }
}
