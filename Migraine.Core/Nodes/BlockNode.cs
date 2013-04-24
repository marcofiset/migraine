using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class BlockNode : Node
    {
        public List<Node> Expressions;

        public BlockNode(List<Node> expressions = null)
        {
            Expressions = expressions ?? new List<Node>();
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
