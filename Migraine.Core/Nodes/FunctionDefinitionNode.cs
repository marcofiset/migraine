using Migraine.Core.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Nodes
{
    public class FunctionDefinitionNode : Node
    {
        public String Name { get; private set; }
        public List<String> Arguments { get; private set; }
        public BlockNode Body { get; private set; }

        public FunctionDefinitionNode(String name, List<String> arguments, BlockNode body)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public override TReturn Accept<TReturn>(IMigraineAstVisitor<TReturn> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
