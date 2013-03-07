using Migraine.Core.Nodes;
using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class UnaryMinusNode : Node
    {
        public Node Node { get; private set; }

        public UnaryMinusNode(Node node)
        {
            Node = node;
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
