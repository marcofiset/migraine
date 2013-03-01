using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core
{
    public class TokenStreamEmptyException : Exception
    {
        private static String _message = "Can't consume token on an empty stream.";

        public TokenStreamEmptyException() : base(_message) { }
    }

    public class TokenStream
    {
        private Queue<Token> _queue;

        public TokenStream()
        {
            _queue = new Queue<Token>();
        }

        public void Add(Token token)
        {
            _queue.Enqueue(token);
        }

        public Boolean Consume()
        {
            if (_queue.Count == 0)
                throw new TokenStreamEmptyException();

            _queue.Dequeue();
            return true;
        }

        public Boolean Consume(string tokenValue)
        {
            return true;
        }

        public Boolean Consume(TokenType tokenType)
        {
            return true;
        }

        public Int32 Count 
        {
            get { return _queue.Count; }
        }
    }
}
