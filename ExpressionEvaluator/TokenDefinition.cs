using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluator
{
    public class TokenDefinition
    {
        public TokenType Type { get; private set; }
        private IMatchEvaluator matchEvaluator;

        public TokenDefinition(IMatchEvaluator matchEvaluator, TokenType type)
        {
            this.Type = type;
            this.matchEvaluator = matchEvaluator;
        }

        public Match Match(string input)
        {
            return this.matchEvaluator.Match(input);
        }
    }
}
