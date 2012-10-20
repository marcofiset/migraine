using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class TermNode : Node
    {
        private FactorNode leftFactor;
        private String oper;
        private FactorNode rightFactor;

        public TermNode(FactorNode leftFactor, String oper = "", FactorNode rightFactor = null)
        {
            this.leftFactor = leftFactor;
            this.oper = oper;
            this.rightFactor = rightFactor;
        }

        public override double Evaluate()
        {
            var leftResult = leftFactor.Evaluate();
            if (rightFactor == null) return leftResult;

            var rightResult = rightFactor.Evaluate();

            switch (oper)
            {
                case "*":
                    return leftResult * rightResult;

                case "/":
                    return leftResult / rightResult;

                default:
                    throw new Exception("Unexpected operator");
            }
        }
    }
}
