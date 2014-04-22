using Migraine.Core.Nodes;
using Migraine.Core.Visitors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Migraine.Core.Exceptions;

namespace Migraine.Core.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private TokenStream tokenStream;
        private MigraineLexer lexer;

        [SetUp]
        public void SetUp()
        {
            tokenStream = new TokenStream();
            lexer = new MigraineLexer();
        }

        private Double EvaluateExpression(String expression, Dictionary<String, FunctionDefinitionNode> functions = null)
        {
            if (functions == null)
                functions = new Dictionary<String, FunctionDefinitionNode>();

            tokenStream = lexer.Tokenize(expression);
            var parser = new Parser(tokenStream);

            var node = parser.Parse();
            var symbolParser = new SymbolTableParser();
            node.Accept(symbolParser);

            return node.Accept(new MigraineInterpreter(symbolParser.functions));
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

        [Test]
        public void TestVariableAssignment()
        {
            Assert.AreEqual(2.625, EvaluateExpression("x = 2.625"));
        }

        [Test]
        public void TestUseVariableInExpression()
        {
            Assert.AreEqual(-12, EvaluateExpression(
                @"x = 12; 
                  y = 2 * x; 
                  z = -y / 2;"));
        }

        [Test]
        public void TestMultipleAssignment()
        {
            Assert.AreEqual(10, EvaluateExpression("x = y = 5; x + y"));
        }

        [Test]
        public void TestSimpleFunction()
        {
           var expression = @"
                fun add(n1, n2) {
                    n1 + n2
                }

                add(3, 8);
           ";

            Assert.AreEqual(11, EvaluateExpression(expression));
        }

        [Test]
        public void MultipleFunctionCalls()
        {
            var expression = @"
                fun square(n) {
                    n * n
                }

                fun add2(n) {
                    n + 2
                }

                square(5) * add2(4) + square(2);
            ";

            Assert.AreEqual(154, EvaluateExpression(expression));
        }

        [Test]
        public void FunctionCallsAsArguments()
        {
            var expression = @"
                fun add(n1, n2) {
                    n1 + n2
                }

                fun square(n) {
                    n * n
                }

                add(square(add(square(2), 2)), square(add(3, 2)))
            ";

            Assert.AreEqual(61, EvaluateExpression(expression));
        }

        [Test]
        public void FunctionsCanBeCalledBeforeBeingDefined()
        {
            var expression = @"
                add(1, 2);
                
                fun add(n1, n2) { n1 + n2 }
            ";

            Assert.AreEqual(0, EvaluateExpression(expression));
        }

        [Test]
        public void FunctionParametersDontOverwriteParentScopeVariables()
        {
            var expression = @"
                n1 = 5;
                n2 = 6;

                fun add(n1, n2) { n1 + n2 }
                
                add(7, 8);
                n1 + n2
            ";

            Assert.AreEqual(11, EvaluateExpression(expression));
        }

        [Test]
        public void BlocksHaveTheirOwnScope()
        {
            var expression = @"
                { n = 6 }

                n
            ";

            Assert.Throws<UndefinedIdentifier>(() => EvaluateExpression(expression));
        }

        [Test]
        public void CallingUndefinedFunctionShouldFail()
        {
            var expression = "add(2, 4)";

            Assert.Throws<UndefinedIdentifier>(() => EvaluateExpression(expression));
        }

        [Test]
        public void CallingFunctionWithWrongNumberOfArgumentsShouldFail()
        {
            var expression = @"
                fun add(n1, n2) { n1 + n2 }
                add(5)
            ";

            Assert.Throws<BadFunctionCall>(() => EvaluateExpression(expression));
        }

        [Test]
        public void UnaryMinusWithFunctionCall()
        {
            var expression = @"
                fun add(n1, n2) { n1 + n2 }
                -add(5, 3)
            ";

            Assert.AreEqual(-8, EvaluateExpression(expression));
        }

        [Test]
        public void InnerFunctions()
        {
            var expression = @"
                fun add2ThenSquare(n) {
                    fun add2(n) {
                        n + 2
                    }

                    nn = add2(n);
                    nn * nn
                }

                add2ThenSquare(4)
            ";

            Assert.AreEqual(36, EvaluateExpression(expression));
        }

        [Test]
        public void ContentOfIfBlockNotExecutedIfConditionNotMet()
        {
            var expression = @"
                n = 5;
                
                if (0) {
                    n = 6;
                }

                n
            ";

            Assert.AreEqual(5, EvaluateExpression(expression));
        }

        [Test]
        public void IfBlockExecutedIfConditionMet()
        {
            var expression = @"
                n = 5;
                
                if (n == 5) {
                    n = 6;
                }

                n
            ";

            Assert.AreEqual(6, EvaluateExpression(expression));
        }
    }
}