using System;

namespace ExpressionEvaluator
{
    public class Token
    {
        public String Value { get; private set; }
        public TokenType Type { get; private set; }

        public Token(String value, TokenType type)
        {
        	if (value == null) throw new ArgumentNullException("value");
        	
            this.Value = value;
            this.Type = type;
        }
    }
}