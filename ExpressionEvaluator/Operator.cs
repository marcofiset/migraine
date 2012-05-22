using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionEvaluator
{
    public class Operator
    {
        public int Precedence { get; set; }
        public Associativity Associativity { get; set; }
        public string Symbol { get; set; }
        public int ArgumentsCount { get; set; }

        public Operator(string symbol, int precedence, Associativity associativity, int argCount)
        {
            this.Symbol = symbol;
            this.Precedence = precedence;
            this.Associativity = associativity;
            this.ArgumentsCount = argCount;
        }
    }
}
