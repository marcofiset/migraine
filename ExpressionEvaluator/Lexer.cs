using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluator
{
    public class Lexer
    {
        List<TokenDefinition> tokenDefinitions;

        public Lexer(IEnumerable<TokenDefinition> tokenDefinitions)
        {
            this.tokenDefinitions = tokenDefinitions as List<TokenDefinition>;
        }

        public IEnumerable<Token> Tokenize(String input)
        {
            List<Token> tokens = new List<Token>();

            int currentPosition = 0;

            while (currentPosition < input.Length)
            {
                int previousPosition = currentPosition;

                foreach (TokenDefinition tokenDef in tokenDefinitions)
                {
                    Match match = tokenDef.Match(input.Substring(currentPosition));

                    if (match.Success && match.Index == 0)
                    {
                        tokens.Add(new Token(match.Value, tokenDef.Type));
                        currentPosition += match.Length;
                        break;
                    }
                }

                if (previousPosition == currentPosition)
                {
                    throw new Exception(String.Format("Unexpected token at position {0}", currentPosition));
                }
            }

            return tokens;
        }
    }
}
