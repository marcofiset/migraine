using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class UnaryMinusNode : Node
    {
        private Node _node;

        public UnaryMinusNode(Node node)
        {
            _node = node;
        }

        public override double Evaluate()
        {
            return -_node.Evaluate();
        }
    }
}
