using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Migraine.Core
{
    public class TokenDefinition
    {
        public TokenType Type { get; private set; }
        private Regex regex;

        public TokenDefinition(Regex regex, TokenType type)
        {
            this.Type = type;
            this.regex = regex;
        }

        public Match Match(string input)
        {
            return regex.Match(input);
        }
    }
}
