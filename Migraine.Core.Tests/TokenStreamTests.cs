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

        [SetUp]
        public void SetUp()
        {
            _tokenStream = new TokenStream();
        }

        [Test]
        public void CanAddTokenToStream()
        {
            _tokenStream.Add(new Token("5", TokenType.Number));
        }

        [Test]
        public void StreamCountRaiseWhenAdd()
        {
            Assert.AreEqual(0, _tokenStream.Count);
            _tokenStream.Add(new Token("5", TokenType.Number));
            Assert.AreEqual(1, _tokenStream.Count);
        }

        [Test]
        public void StreamCanConsumeAnyToken()
        {
            _tokenStream.Add(new Token("5", TokenType.Number));
            Assert.That(_tokenStream.Consume());
        }

        [Test]
        public void StreamCanConsumeTokenByValue()
        {
            _tokenStream.Add(new Token("5", TokenType.Number));
            Assert.That(_tokenStream.Consume("5"));
        }

        [Test]
        public void StreamCanConsumeTokenByType()
        {
            _tokenStream.Add(new Token("5", TokenType.Number));
            Assert.That(_tokenStream.Consume(TokenType.Number));
        }

        [Test]
        public void ConsumeShouldReduceCountByOne()
        {
            _tokenStream.Add(new Token("5", TokenType.Number));
            _tokenStream.Consume();
            Assert.AreEqual(0, _tokenStream.Count);
        }

        [Test]
        public void ConsumeOnEmptyStreamThrowsException()
        {
            Assert.Throws(typeof(TokenStreamEmptyException), () => _tokenStream.Consume());
        }
    }
}
