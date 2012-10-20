using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class ExpressionNode : Node
    {
        private TermNode leftTerm;
        private TermNode rightTerm;
        private String oper;

        public ExpressionNode(TermNode leftTerm, String op = "", TermNode rightTerm = null)
        {
            this.leftTerm = leftTerm;
            this.oper = op;
            this.rightTerm = rightTerm;
        }

        public override double Evaluate()
        {
            double leftResult = leftTerm.Evaluate();

            if (rightTerm == null)
                return leftResult;

            double rightResult = rightTerm.Evaluate();

            //There will never be any other operator here, 
            //so it's fine to take this unflexible approach
            switch (oper)
            {
                case "+":
                    return leftResult + rightResult;

                case "-":
                    return leftResult - rightResult;

                default:
                    throw new Exception("Unexpected operator");
            }
        }
    }
}
