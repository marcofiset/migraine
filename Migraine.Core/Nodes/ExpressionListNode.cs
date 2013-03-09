using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class ExpressionListNode : Node
    {
        public IEnumerable<Node> Expressions;

        public ExpressionListNode(IEnumerable<Node> nodes)
        {
            Expressions = nodes as List<Node>;
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
