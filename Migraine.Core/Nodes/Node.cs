using Migraine.Core.Nodes;
using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public abstract class Node : IVisitable
    {
        public abstract TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor);
    }
}
