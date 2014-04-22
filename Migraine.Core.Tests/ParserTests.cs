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
        private Parser parser;
        private TokenStream tokens;

        [SetUp]
        public void SetUp()
        {
            tokens = new TokenStream();
            parser = new Parser(tokens);
        }

        [Test]
        public void CanParseNumberNode()
        {
            tokens.Add(new Token("32", TokenType.Number));

            var node = parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<NumberNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseUnaryMinusNode()
        {
            tokens.Add(new Token("-", TokenType.Operator));
            tokens.Add(new Token("23.5", TokenType.Number));

            var node = parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<UnaryMinusNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseOperationNode()
        {
            tokens.Add(new Token("32", TokenType.Number));
            tokens.Add(new Token("-", TokenType.Operator));
            tokens.Add(new Token("24", TokenType.Number));

            var node = parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<OperationNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseExpressionListNode()
        {
            tokens.Add(new Token("32", TokenType.Number));
            tokens.Add(new Token("-", TokenType.Operator));
            tokens.Add(new Token("24", TokenType.Number));

            tokens.Add(new Token(@"\n", TokenType.Terminator));

            tokens.Add(new Token("8", TokenType.Number));
            tokens.Add(new Token("*", TokenType.Operator));
            tokens.Add(new Token("9", TokenType.Number));

            var node = parser.Parse();

            Assert.IsInstanceOf<ExpressionListNode>(node);
            Assert.AreEqual(2, (node as ExpressionListNode).Expressions.Count());
        }

        [Test]
        public void CanParseIdentifierNode()
        {
            tokens.Add(new Token("number", TokenType.Identifier));
            var node = parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<IdentifierNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseAssignmentNode()
        {
            tokens.Add(new Token("var1", TokenType.Identifier));
            tokens.Add(new Token("=", TokenType.Operator));
            tokens.Add(new Token("2.75", TokenType.Number));

            var node = parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<AssignmentNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseComplexAssignmentNode()
        {
            tokens.Add(new Token("var1", TokenType.Identifier));
            tokens.Add(new Token("=", TokenType.Operator));
            tokens.Add(new Token("2.75", TokenType.Number));
            tokens.Add(new Token("*", TokenType.Operator));
            tokens.Add(new Token("x", TokenType.Identifier));

            var node = parser.Parse() as ExpressionListNode;

            Assert.IsInstanceOf<AssignmentNode>(node.Expressions.First());
        }

        [Test]
        public void CanParseEmptyFunctionCall()
        {
            tokens.Add(new Token("function", TokenType.Identifier));
            tokens.Add(new Token("(", TokenType.Operator));
            tokens.Add(new Token(")", TokenType.Operator));

            var node = parser.Parse() as ExpressionListNode;
            var functionCall = node.Expressions.First() as FunctionCallNode;

            Assert.IsNotNull(functionCall);
            Assert.AreEqual("function", functionCall.Name);
            Assert.AreEqual(0, functionCall.Arguments.Count);
        }

        [Test]
        public void CanParseFunctionCallWithArguments()
        {
            tokens.Add(new Token("function", TokenType.Identifier));
            tokens.Add(new Token("(", TokenType.Operator));
            tokens.Add(new Token("5", TokenType.Number));
            tokens.Add(new Token("+", TokenType.Operator));
            tokens.Add(new Token("7", TokenType.Number));
            tokens.Add(new Token(",", TokenType.Operator));
            tokens.Add(new Token("var1", TokenType.Identifier));
            tokens.Add(new Token(")", TokenType.Operator));

            var node = parser.Parse() as ExpressionListNode;
            var functionCall = node.Expressions.First() as FunctionCallNode;

            Assert.IsNotNull(functionCall);
            Assert.AreEqual("function", functionCall.Name);
            Assert.AreEqual(2, functionCall.Arguments.Count);
        }

        [Test]
        public void CanParseEmptyFunctionDefinition()
        {
            tokens.Add(new Token("fun", TokenType.Identifier));
            tokens.Add(new Token("add", TokenType.Identifier));
            tokens.Add(new Token("(", TokenType.Operator));
            tokens.Add(new Token("var1", TokenType.Identifier));
            tokens.Add(new Token(",", TokenType.Operator));
            tokens.Add(new Token("var2", TokenType.Identifier));
            tokens.Add(new Token(")", TokenType.Operator));
            tokens.Add(new Token("{", TokenType.Operator));
            tokens.Add(new Token("}", TokenType.Operator));

            var node = parser.Parse() as ExpressionListNode;
            var functionDef = node.Expressions.First() as FunctionDefinitionNode;

            Assert.IsNotNull(functionDef);
        }

        [Test]
        public void CanParseNonEmptyFunctionDefinition()
        {
            tokens.Add(new Token("fun", TokenType.Identifier));
            tokens.Add(new Token("add", TokenType.Identifier));
            tokens.Add(new Token("(", TokenType.Operator));
            tokens.Add(new Token("var1", TokenType.Identifier));
            tokens.Add(new Token(",", TokenType.Operator));
            tokens.Add(new Token("var2", TokenType.Identifier));
            tokens.Add(new Token(")", TokenType.Operator));
            tokens.Add(new Token("{", TokenType.Operator));
            tokens.Add(new Token("var1", TokenType.Identifier));
            tokens.Add(new Token("+", TokenType.Operator));
            tokens.Add(new Token("var2", TokenType.Identifier));
            tokens.Add(new Token(";", TokenType.Terminator));
            tokens.Add(new Token("}", TokenType.Operator));

            var node = parser.Parse() as ExpressionListNode;
            var functionDef = node.Expressions.First() as FunctionDefinitionNode;

            Assert.IsNotNull(functionDef);
        }

        [Test]
        public void CanParseIfStatement()
        {
            tokens.Add(new Token("if", TokenType.Identifier));
            tokens.Add(new Token("(", TokenType.Operator));
            tokens.Add(new Token("1", TokenType.Number));
            tokens.Add(new Token(")", TokenType.Operator));
            tokens.Add(new Token("{", TokenType.Operator));
            tokens.Add(new Token("n", TokenType.Identifier));
            tokens.Add(new Token("=", TokenType.Operator));
            tokens.Add(new Token("6", TokenType.Number));
            tokens.Add(new Token("}", TokenType.Operator));

            var node = parser.Parse() as ExpressionListNode;
            var ifStatement = node.Expressions.First() as IfStatementNode;

            Assert.IsNotNull(ifStatement);
        }
    }
}
