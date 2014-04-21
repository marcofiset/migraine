using Migraine.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Visitors
{
    public class SymbolTableParser : IMigraineAstVisitor<Double>
    {
        public Dictionary<String, FunctionDefinitionNode> functions
        {
            get;
            private set;
        }

        public SymbolTableParser()
        {
            functions = new Dictionary<String, FunctionDefinitionNode>();
        }

        public double Visit(NumberNode node)
        {
            return 0;
        }

        public double Visit(UnaryMinusNode node)
        {
            return 0;
        }

        public double Visit(OperationNode node)
        {
            return 0;
        }

        public double Visit(ExpressionListNode node)
        {
            foreach (var expr in node.Expressions)
                expr.Accept(this);

            return 0;
        }

        public double Visit(AssignmentNode assignmentNode)
        {
            return 0;
        }

        public double Visit(IdentifierNode identifierNode)
        {
            return 0;
        }

        public double Visit(BlockNode blockNode)
        {
            foreach (var expr in blockNode.Expressions)
                expr.Accept(this);

            return 0;
        }

        public double Visit(FunctionCallNode functionCallNode)
        {
            return 0;
        }

        public double Visit(FunctionDefinitionNode functionDefinitionNode)
        {
            var name = functionDefinitionNode.Name;

            if (functions.ContainsKey(name))
                throw new Exception(String.Format("Method {0} is already defined.", name));

            functions.Add(name, functionDefinitionNode);

            return 0;
        }
    }
}
