using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class OperationNode : Node
    {
        private Node leftNode;
        private List<Tuple<string, Node>> restOfExpression;
        
        public OperationNode(Node leftNode, IEnumerable<Tuple<string, Node>> restOfExpression)
        {
            if (leftNode == null) throw new ArgumentNullException("leftNode");
            if (restOfExpression == null) throw new ArgumentNullException("restOfExpression");

            this.leftNode = leftNode;
            this.restOfExpression = restOfExpression as List<Tuple<string, Node>>;
        }

        public override double Evaluate()
        {
            double result = leftNode.Evaluate();

            foreach (var pair in restOfExpression)
            {
                var op = pair.Item1;
                var right = pair.Item2;

                //There won't be any other operator here so this unflexible
                //approach is fine. I decided to keep it simple
                switch (op)
                {
                    case "+":
                        result += right.Evaluate();
                        break;

                    case "-":
                        result -= right.Evaluate();
                        break;

                    case "*":
                        result *= right.Evaluate();
                        break;

                    case "/":
                        result /= right.Evaluate();
                        break;

                    default:
                        throw new Exception("Unsupported operator : " + op);
                }
            }

            return result;
        }
    }
}
