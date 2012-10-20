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
        public void TestSimpleExpression()
        { 
            var tokenQueue = new Queue<Token>();
            tokenQueue.Enqueue(new Token("32", TokenType.Number));
            tokenQueue.Enqueue(new Token("/", TokenType.Operator));
            tokenQueue.Enqueue(new Token("-", TokenType.Operator));
            tokenQueue.Enqueue(new Token("8", TokenType.Number));

            var parser = new Parser(tokenQueue);
            var rootNode = parser.Parse();

            Assert.AreEqual(-32, rootNode.Evaluate());
        }
    }
}
