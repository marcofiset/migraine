﻿using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class OperationNode : Node
    {
        public Node LeftNode { get; private set; }
        public IEnumerable<Tuple<string, Node>> RestOfExpression { get; private set; }

        public OperationNode(Node leftNode, IEnumerable<Tuple<string, Node>> restOfExpression)
        {
            if (leftNode == null) throw new ArgumentNullException("leftNode");
            if (restOfExpression == null) throw new ArgumentNullException("restOfExpression");

            LeftNode = leftNode;
            RestOfExpression = restOfExpression;
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
