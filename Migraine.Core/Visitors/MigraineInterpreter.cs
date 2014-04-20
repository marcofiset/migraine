using Migraine.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Migraine.Core.Exceptions;

namespace Migraine.Core.Visitors
{
    public class MigraineInterpreter : IMigraineAstVisitor<Double>
    {
        private Stack<Scope> scopes;
        private Dictionary<String, FunctionDefinitionNode> functions;

        public Scope CurrentScope
        {
            get { return scopes.Peek(); }
        }

        public MigraineInterpreter(Dictionary<String, FunctionDefinitionNode> functions)
        {
            scopes = new Stack<Scope>();
            scopes.Push(new Scope());

            this.functions = functions;
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

            CurrentScope.AssignVariable(name, result);

            return result;
        }

        public Double Visit(IdentifierNode identifierNode)
        {
            return CurrentScope.ResolveVariable(identifierNode.Name);
        }

        public Double Visit(FunctionDefinitionNode functionDefinitionNode)
        {
            //Do nothing, we already have the functions here.
            return 0;
        }

        public Double Visit(BlockNode blockNode)
        {
            return WithNewScopeDo(scope =>
            {
                double lastValue = 0;

                foreach (var exp in blockNode.Expressions)
                    lastValue = exp.Accept(this);

                return lastValue;
            });
            
        }

        public Double Visit(FunctionCallNode functionCallNode)
        {
            var functionDefinition = ValidateFunctionCall(functionCallNode);

            return WithNewScopeDo(scope =>
            {
                for (int i = 0; i < functionCallNode.Arguments.Count; i++)
                {
                    var name = functionDefinition.Arguments[i];
                    var value = functionCallNode.Arguments[i].Accept(this);

                    scope.DefineVariable(name, value);
                }

                return functionDefinition.Body.Accept(this);
            });
        }

        private FunctionDefinitionNode ValidateFunctionCall(FunctionCallNode functionCallNode)
        {
            var functionName = functionCallNode.Name;

            if (!functions.ContainsKey(functionName))
                throw new UndefinedFunction(functionName);

            var functionDefinition = functions[functionName];

            if (functionDefinition.Arguments.Count != functionCallNode.Arguments.Count)
                throw new BadFunctionCall(functionName, functionDefinition.Arguments.Count, functionCallNode.Arguments.Count);

            return functionDefinition;
        }

        private Double WithNewScopeDo(Func<Scope, Double> action)
        {
            var newScope = new Scope(CurrentScope);

            scopes.Push(newScope);
            Double result = action(newScope);
            scopes.Pop();

            return result;
        }
    }
}