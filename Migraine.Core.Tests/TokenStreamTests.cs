using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Tests
{
    [TestFixture]
    public class TokenStreamTests
    {
        private TokenStream tokenStream;
        private Token token;

        [SetUp]
        public void SetUp()
        {
            tokenStream = new TokenStream();
            token = new Token("5", TokenType.Number);
            tokenStream.Add(token);
        }

        [Test]
        public void StreamCountRaiseWhenAdd()
        {
            Assert.AreEqual(1, tokenStream.Count);
            tokenStream.Add(token);
            Assert.AreEqual(2, tokenStream.Count);
        }

        [Test]
        public void StreamCanConsumeAnyToken()
        {
            Assert.IsTrue(tokenStream.Consume());
        }

        [Test]
        public void StreamCanConsumeTokenByValue()
        {
            Assert.IsTrue(tokenStream.Consume("5"));
        }

        [Test]
        public void StreamCanConsumeTokenByType()
        {
            Assert.IsTrue(tokenStream.Consume(TokenType.Number));
        }

        [Test]
        public void CanAccessCurrentToken()
        {
            Assert.AreEqual(token, tokenStream.CurrentToken);
        }

        [Test]
        public void CurrentTokenThrowsExceptionIfStreamEmpty()
        {
            tokenStream.Consume();
            Assert.Throws<TokenStreamEmptyException>(() => { var tok = tokenStream.CurrentToken; });
        }

        [Test]
        public void ConsumeShouldReduceCountByOne()
        {
            tokenStream.Consume();
            Assert.AreEqual(0, tokenStream.Count);
        }

        [Test]
        public void ConsumeWrongTypeReturnsFalse()
        {
            Assert.IsFalse(tokenStream.Consume(TokenType.Operator));
        }

        [Test]
        public void ConsumeWrongValueReturnsFalse()
        {
            Assert.IsFalse(tokenStream.Consume("+"));
        }

        [Test]
        public void ConsumeOnEmptyStreamThrowsException()
        {
            tokenStream.Consume();
            Assert.Throws(typeof(TokenStreamEmptyException), () => tokenStream.Consume());
        }

        [Test]
        public void CanAccessConsumedToken()
        {
            var currentToken = tokenStream.CurrentToken;
            tokenStream.Consume();
            Assert.AreEqual(currentToken, tokenStream.ConsumedToken);
        }

        [Test]
        public void ExpectBehavesAsConsume()
        {
            var currentToken = tokenStream.CurrentToken;
            Assert.IsTrue(tokenStream.Expect(TokenType.Number));

            tokenStream.Add(currentToken);
            Assert.IsTrue(tokenStream.Expect("5"));
        }

        [Test]
        public void ExpectThrowsExceptionIfWrongTypeOrValue()
        {
            Assert.Throws<ExpectedTokenException>(() => tokenStream.Expect("+"));
            Assert.Throws<ExpectedTokenException>(() => tokenStream.Expect(TokenType.Operator));
        }

        [Test]
        public void CanConsumeAnyTokenValueFromList()
        {
            Assert.IsFalse(tokenStream.ConsumeAny("-", "10", ")"));
            Assert.IsTrue(tokenStream.ConsumeAny("+", "5"));
        }

        [Test]
        public void CanConsumeAnyTokenTypeFromList()
        {
            Assert.IsFalse(tokenStream.ConsumeAny(TokenType.Identifier, TokenType.Operator));
            Assert.IsTrue(tokenStream.ConsumeAny(TokenType.Operator, TokenType.Number));
        }

        [Test]
        public void CanUseLookAhead()
        {
            tokenStream.Add(new Token("+", TokenType.Operator));

            Assert.AreEqual("+", tokenStream.LookAhead().Value);
            Assert.AreEqual(TokenType.Operator, tokenStream.LookAhead().Type);
        }

        [Test]
        public void CanSpecifyLookAheadPosition()
        {
            var opToken = new Token("+", TokenType.Operator);
            var numToken = new Token("3", TokenType.Number);

            tokenStream.Add(opToken);
            tokenStream.Add(numToken);

            Assert.AreEqual(numToken, tokenStream.LookAhead(2));
        }

        [Test]
        public void LookAheadTooFarReturnsNull()
        {
            Assert.IsNull(tokenStream.LookAhead(2));
        }
    }
}
