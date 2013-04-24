using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class FunctionCallNode : Node
    {
        public String Name { get; private set; }
        public List<Node> Arguments { get; private set; }

        public FunctionCallNode(String name, List<Node> arguments = null)
        {
            Name = name;
            Arguments = arguments ?? new List<Node>();
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
