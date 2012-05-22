using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionEvaluator
{
    public class Match
    {
        public int Index { get; private set; }
        public bool Success { get; private set; }
        public string Value { get; private set; }

        public int Length
        {
            get { return Value.Length; }
        }

        public Match(int index, bool success, string value)
        {
            this.Index = index;
            this.Success = success;
            this.Value = value;
        }
    }
}