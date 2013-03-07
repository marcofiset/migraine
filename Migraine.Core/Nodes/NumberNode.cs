using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class NumberNode : Node
    {
        public Double Value { get; private set; }

        public NumberNode(double value)
        {
            Value = value;
        }

        public override TReturn Accept<TReturn>(Visitors.IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
