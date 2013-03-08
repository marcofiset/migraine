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
        private Token _lastConsumedToken;

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
        /// <returns>True if a token was consumed, false otherwise</returns>
        public Boolean Consume()
        {
            if (IsEmpty)
                throw new TokenStreamEmptyException();

            _lastConsumedToken = _queue.Dequeue();

            return true;
        }

        /// <summary>
        /// Consumes the current token by value
        /// </summary>
        /// <param name="tokenValue">The expected token value</param>
        /// <exception cref="TokenStreamEmptyException">If stream is empty</exception>
        /// <returns>True if a token was consumed, false otherwise</returns>
        public Boolean Consume(String tokenValue)
        {
            if (CurrentToken.Value != tokenValue)
                return false;

            return Consume();
        }

        /// <summary>
        /// Consumes the token with the first matching value encountered.
        /// </summary>
        /// <param name="values">Any number of possible values</param>
        /// <returns>True if any value matched, false if none of the values matched</returns>
        public Boolean ConsumeAny(params string[] values)
        {
            foreach (String value in values)
            {
                if (Consume(value)) return true;
            }

            return false;
        }

        /// <summary>
        /// Consumes the current token by TokenType
        /// </summary>
        /// <param name="tokenType">The expected TokenType</param>
        /// <exception cref="TokenStreamEmptyException">If stream is empty</exception>
        /// <returns>True if a token was consumed, false otherwise</returns>
        public Boolean Consume(TokenType tokenType)
        {
            if (CurrentToken.Type != tokenType)
                return false;

            return Consume();
        }

        /// <summary>
        /// Consumes the token with the first matching type encountered.
        /// </summary>
        /// <param name="types">Any number of possible types</param>
        /// <returns>True if any type matched, false if none of the types matched</returns>
        public Boolean ConsumeAny(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Consume(type)) return true;
            }

            return false;
        }

        /// <summary>
        /// Expects the current token's type to be of a specific type.
        /// If CurrentToken's type is of the wrong type, an ExpectedTokenTokenException is thrown.
        /// </summary>
        /// <param name="type">The expected TokenType</param>
        /// <exception cref="ExpectedTokenException"></exception>
        /// <returns>A Boolean indicating whether the current token is of the specified type</returns>
        public Boolean Expect(TokenType type)
        {
            if (CurrentToken.Type != type)
                throw new ExpectedTokenException(type);

            return Consume(type);
        }

        /// <summary>
        /// Expects the current token's value to be of a specific value.
        /// If CurrentToken's value is of the wrong value, an ExpectedTokenTokenException is thrown.
        /// </summary>
        /// <param name="type">The expected value</param>
        /// <exception cref="ExpectedTokenException"></exception>
        /// <returns>A Boolean indicating whether the current token is of the specified type</returns>
        public Boolean Expect(String value)
        {
            if (CurrentToken.Value != value)
                throw new ExpectedTokenException(value);

            return Consume(value);
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

        /// <summary>
        /// Gets the token that was last consumed
        /// </summary>
        public Token ConsumedToken 
        {
            get { return _lastConsumedToken; } 
        }
    }
}
