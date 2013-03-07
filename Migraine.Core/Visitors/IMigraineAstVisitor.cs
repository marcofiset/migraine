using Migraine.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Visitors
{
    public interface IMigraineAstVisitor<TReturn>
    {
        TReturn Visit(NumberNode node);
        TReturn Visit(UnaryMinusNode node);
        TReturn Visit(OperationNode node);
    }
}
