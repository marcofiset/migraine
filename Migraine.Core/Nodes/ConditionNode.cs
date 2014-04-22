using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Migraine.Core.Visitors;

namespace Migraine.Core.Nodes
{
    public class ConditionNode : Node
    {
        public Node LeftOperand { get; private set; }
        public String Operator { get; private set; }
        public Node RightOperand { get; private set; }

        public ConditionNode(Node left, String comparisonOperator = null, Node right = null)
        {
            LeftOperand = left;
            Operator = comparisonOperator;
            RightOperand = right;
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
