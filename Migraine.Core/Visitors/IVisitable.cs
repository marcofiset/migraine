using Migraine.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Visitors
{
    public interface IVisitable
    {
        TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor);
    }
}
