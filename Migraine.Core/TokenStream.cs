using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core
{
    public class TokenStreamEmptyException : Exception
    {
        public TokenStreamEmptyException() : base("Can't consume token on an empty stream.") { }
    }

    public class ExpectedTokenException : Exception
    {
        public ExpectedTokenException(String expectedValue) 
            : base("Expected token value to be " + expectedValue) { }

        public ExpectedTokenException(TokenType expectedType) 
            : base("Expected token type to be " + expectedType.ToString()) { }
    }

    public class TokenStream
    {
        private Queue<Token> _queue;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TokenStream()
        {
            _queue = new Queue<Token>();
        }

        /// <summary>
        /// Adds a token to the stream
        /// </summary>
        /// <param name="token">The token to add</param>
        public void Add(Token token)
        {
            _queue.Enqueue(token);
        }

        /// <summary>
        /// Consumes the current token regardless of its value or type.
        /// </summary>
        /// <exception cref="TokenStreamEmptyException">If stream is empty</exception>
        public void Consume()
        {
            if (IsEmpty)
                throw new TokenStreamEmptyException();

            _queue.Dequeue();
        }

        /// <summary>
        /// Consumes the current token by value
        /// </summary>
        /// <param name="tokenValue">The expected token value</param>
        /// <exception cref="ExpectedTokenException">If current token's value is not the expected one</exception>
        /// <exception cref="TokenStreamEmptyException">If stream is empty</exception>
        public void Consume(string tokenValue)
        {
            if (CurrentToken.Value != tokenValue)
                throw new ExpectedTokenException(tokenValue);

            Consume();
        }

        /// <summary>
        /// Consumes the current token by TokenType
        /// </summary>
        /// <param name="tokenType">The expected TokenType</param>
        /// <exception cref="ExpectedTokenException">If current token's type is not the expected one</exception>
        /// <exception cref="TokenStreamEmptyException">If stream is empty</exception>
        public void Consume(TokenType tokenType)
        {
            if (CurrentToken.Type != tokenType)
                throw new ExpectedTokenException(tokenType);

            Consume();
        }

        /// <summary>
        /// Gets the number of tokens in the stream
        /// </summary>
        public Int32 Count 
        {
            get { return _queue.Count; }
        }
        
        /// <summary>
        /// Gets whether the stream is empty or not
        /// </summary>
        public Boolean IsEmpty
        {
            get { return Count == 0; }
        }

        /// <summary>
        /// Gets the current token. If the stream is empty, null is returned.
        /// </summary>
        public Token CurrentToken
        {
            get { return IsEmpty ? null : _queue.Peek(); }
        }
    }
}
