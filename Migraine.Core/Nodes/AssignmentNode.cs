using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class AssignmentNode : Node
    {
        public String Name { get; private set; }
        public Node Expression { get; private set; }

        public AssignmentNode(String name, Node expression)
        {
            Name = name;
            Expression = expression;
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
