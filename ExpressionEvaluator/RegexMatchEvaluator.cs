using System;
using System.Text.RegularExpressions;

namespace ExpressionEvaluator
{
    public class RegexMatchEvaluator : IMatchEvaluator
    {
        private Regex regex;

        public RegexMatchEvaluator(string regexPattern)
        {
            this.regex = new Regex(regexPattern);
        }

        public Match Match(string input)
        {
            System.Text.RegularExpressions.Match regexMatch = regex.Match(input);
            return new Match(regexMatch.Index, regexMatch.Success, regexMatch.Value);
        }
    }
}