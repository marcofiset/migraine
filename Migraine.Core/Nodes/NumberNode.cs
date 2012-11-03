using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class NumberNode : Node
    {
        private double value;

        public NumberNode(double value)
        {
            this.value = value;
        }

        public override double Evaluate()
        {
            return value;
        }
    }
}
