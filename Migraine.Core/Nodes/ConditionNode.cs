using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Migraine.Core.Visitors;

namespace Migraine.Core.Nodes
{
    public class ConditionNode : Node
    {
        public ConditionNode(Node left, String comparisonOperator, Node right)
        {

        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
