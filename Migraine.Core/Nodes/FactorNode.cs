using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class FactorNode : Node
    {
        private Double value;

        public FactorNode(Double value)
        {
            this.value = value;
        }

        public override double Evaluate()
        {
            return value;
        }
    }
}
