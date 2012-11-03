using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class OperationNode : Node
    {
        private string op;
        private Node leftNode;
        private Node rightNode;

        public OperationNode(Node leftNode, string op, Node rightNode)
        {
            if (leftNode == null) throw new ArgumentNullException("leftNode");
            if (rightNode == null) throw new ArgumentNullException("rightNode");
            if (!"+-*/".Contains(op)) throw new ArgumentOutOfRangeException("Unsupported operator : " + op);

            this.op = op;
            this.leftNode = leftNode;
            this.rightNode = rightNode;
        }

        public override double Evaluate()
        {
            double left = leftNode.Evaluate();
            double right = rightNode.Evaluate();

            //There won't be any other operator here so this unflexible
            //approach is fine. I decided to keep it simple
            switch (op)
            {
                case "+":
                    return left + right;

                case "-":
                    return left - right;

                case "*":
                    return left * right;

                case "/":
                    return left / right;

                default:
                    throw new Exception("Unsupported operator : " + op);
            }
        }
    }
}