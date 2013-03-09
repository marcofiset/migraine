using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class IdentifierNode : Node
    {
        public String Name { get; private set; }

        public IdentifierNode(String name)
        {
            Name = name;
        }

        public override TReturn Accept<TReturn>(Visitors.IMigraineAstVisitor<TReturn> visitor)
        {
            throw new NotImplementedException();
        }
    }
}
