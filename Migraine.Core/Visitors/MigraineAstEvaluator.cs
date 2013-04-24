using Migraine.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Visitors
{
    public class MigraineAstEvaluator : IMigraineAstVisitor<Double>
    {
        private Dictionary<String, Double> variables;

        public MigraineAstEvaluator()
        {
            variables = new Dictionary<String, Double>();
        }

        public Double Visit(NumberNode node)
        {
            return node.Value;
        }

        public Double Visit(UnaryMinusNode node)
        {
            return -node.Node.Accept(this);
        }

        public Double Visit(OperationNode node)
        {
            Double result = node.LeftNode.Accept(this);

            foreach (var pair in node.RestOfExpression)
            {
                var op = pair.Item1;
                var rightNode = pair.Item2;
                var rightValue = rightNode.Accept(this);

                //There won't be any other operator here so this unflexible
                //approach is fine. I decided to keep it simple
                switch (op)
                {
                    case "+":
                        result += rightValue;
                        break;

                    case "-":
                        result -= rightValue;
                        break;

                    case "*":
                        result *= rightValue;
                        break;

                    case "/":
                        result /= rightValue;
                        break;

                    default:
                        throw new Exception("Unsupported operator : " + op);
                }
            }

            return result;
        }

        public Double Visit(ExpressionListNode node)
        {
            Double lastValue = 0;

            foreach (var expression in node.Expressions)
            {
                lastValue = expression.Accept(this);
            }

            return lastValue;
        }

        public Double Visit(AssignmentNode assignmentNode)
        {
            var result = assignmentNode.Expression.Accept(this);
            var name = assignmentNode.Name;

            if (!variables.ContainsKey(name))
                variables.Add(name, result);
            else
                variables[name] = result;

            return result;
        }

        public Double Visit(IdentifierNode identifierNode)
        {
            return variables[identifierNode.Name];
        }

        public Double Visit(FunctionDefinitionNode functionDefinitionNode)
        {
            throw new NotImplementedException();
        }

        public Double Visit(BlockNode blockNode)
        {
            throw new NotImplementedException();
        }

        public Double Visit(FunctionCallNode functionCallNode)
        {
            throw new NotImplementedException();
        }
    }
}