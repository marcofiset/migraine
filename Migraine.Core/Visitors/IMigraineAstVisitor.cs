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
        TReturn Visit(ExpressionListNode node);
        TReturn Visit(AssignmentNode assignmentNode);
        TReturn Visit(IdentifierNode identifierNode);
        TReturn Visit(FunctionDefinitionNode functionDefinitionNode);
        TReturn Visit(BlockNode blockNode);
        TReturn Visit(FunctionCallNode functionCallNode);
        TReturn Visit(IfStatementNode ifStatementNode);
    }
}
