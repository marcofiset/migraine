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

        public override bool Equals(object obj)
		{
			Token other = obj as Token;
			
			if (other == null)
				return false;
			
			return this.Value == other.Value && this.Type == other.Type;
		}
        
		public override int GetHashCode()
		{
			unchecked
		    {
		        int hash = 17;
		        
		        hash = hash * 23 + this.Value.GetHashCode();
		        hash = hash * 23 + this.Type.GetHashCode();
		        
		        return hash;
		    }
		}

    }
}