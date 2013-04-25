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
        private TokenStream _tokenStream;
        private Token _token;

        [SetUp]
        public void SetUp()
        {
            _tokenStream = new TokenStream();
            _token = new Token("5", TokenType.Number);
            _tokenStream.Add(_token);
        }

        [Test]
        public void StreamCountRaiseWhenAdd()
        {
            Assert.AreEqual(1, _tokenStream.Count);
            _tokenStream.Add(_token);
            Assert.AreEqual(2, _tokenStream.Count);
        }

        [Test]
        public void StreamCanConsumeAnyToken()
        {
            Assert.IsTrue(_tokenStream.Consume());
        }

        [Test]
        public void StreamCanConsumeTokenByValue()
        {
            Assert.IsTrue(_tokenStream.Consume("5"));
        }

        [Test]
        public void StreamCanConsumeTokenByType()
        {
            Assert.IsTrue(_tokenStream.Consume(TokenType.Number));
        }

        [Test]
        public void CanAccessCurrentToken()
        {
            Assert.AreEqual(_token, _tokenStream.CurrentToken);
        }

        [Test]
        public void CurrentTokenThrowsExceptionIfStreamEmpty()
        {
            _tokenStream.Consume();
            Assert.Throws<TokenStreamEmptyException>(() => { var tok = _tokenStream.CurrentToken; });
        }

        [Test]
        public void ConsumeShouldReduceCountByOne()
        {
            _tokenStream.Consume();
            Assert.AreEqual(0, _tokenStream.Count);
        }

        [Test]
        public void ConsumeWrongTypeReturnsFalse()
        {
            Assert.IsFalse(_tokenStream.Consume(TokenType.Operator));
        }

        [Test]
        public void ConsumeWrongValueReturnsFalse()
        {
            Assert.IsFalse(_tokenStream.Consume("+"));
        }

        [Test]
        public void ConsumeOnEmptyStreamThrowsException()
        {
            _tokenStream.Consume();
            Assert.Throws(typeof(TokenStreamEmptyException), () => _tokenStream.Consume());
        }

        [Test]
        public void CanAccessConsumedToken()
        {
            var currentToken = _tokenStream.CurrentToken;
            _tokenStream.Consume();
            Assert.AreEqual(currentToken, _tokenStream.ConsumedToken);
        }

        [Test]
        public void ExpectBehavesAsConsume()
        {
            var currentToken = _tokenStream.CurrentToken;
            Assert.IsTrue(_tokenStream.Expect(TokenType.Number));

            _tokenStream.Add(currentToken);
            Assert.IsTrue(_tokenStream.Expect("5"));
        }

        [Test]
        public void ExpectThrowsExceptionIfWrongTypeOrValue()
        {
            Assert.Throws<ExpectedTokenException>(() => _tokenStream.Expect("+"));
            Assert.Throws<ExpectedTokenException>(() => _tokenStream.Expect(TokenType.Operator));
        }

        [Test]
        public void CanConsumeAnyTokenValueFromList()
        {
            Assert.IsFalse(_tokenStream.ConsumeAny("-", "10", ")"));
            Assert.IsTrue(_tokenStream.ConsumeAny("+", "5"));
        }

        [Test]
        public void CanConsumeAnyTokenTypeFromList()
        {
            Assert.IsFalse(_tokenStream.ConsumeAny(TokenType.Identifier, TokenType.Operator));
            Assert.IsTrue(_tokenStream.ConsumeAny(TokenType.Operator, TokenType.Number));
        }

        [Test]
        public void CanUseLookAhead()
        {
            _tokenStream.Add(new Token("+", TokenType.Operator));

            Assert.AreEqual("+", _tokenStream.LookAhead().Value);
            Assert.AreEqual(TokenType.Operator, _tokenStream.LookAhead().Type);
        }

        [Test]
        public void CanSpecifyLookAheadPosition()
        {
            var opToken = new Token("+", TokenType.Operator);
            var numToken = new Token("3", TokenType.Number);

            _tokenStream.Add(opToken);
            _tokenStream.Add(numToken);

            Assert.AreEqual(numToken, _tokenStream.LookAhead(2));
        }

        [Test]
        public void LookAheadTooFarReturnsNull()
        {
            Assert.IsNull(_tokenStream.LookAhead(2));
        }
    }
}
