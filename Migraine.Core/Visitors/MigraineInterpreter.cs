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
        private Stack<Scope<Double>> variableScopes;
        private Stack<Scope<FunctionDefinitionNode>> functionScopes;

        public Scope<Double> CurrentVariableScope
        {
            get { return variableScopes.Peek(); }
        }

        private Scope<FunctionDefinitionNode> CurrentFunctionScope
        {
            get { return functionScopes.Peek(); }
        }

        public MigraineInterpreter(Dictionary<String, FunctionDefinitionNode> functions)
        {
            variableScopes = new Stack<Scope<Double>>();
            variableScopes.Push(new Scope<Double>());

            functionScopes = new Stack<Scope<FunctionDefinitionNode>>();
            functionScopes.Push(new Scope<FunctionDefinitionNode>());

            foreach (var func in functions.Keys)
            {
                CurrentFunctionScope.Define(func, functions[func]);
            }
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

            CurrentVariableScope.Assign(name, result);

            return result;
        }

        public Double Visit(IdentifierNode identifierNode)
        {
            return CurrentVariableScope.Resolve(identifierNode.Name);
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
                Double lastValue = 0;

                foreach (var exp in blockNode.Expressions)
                    lastValue = exp.Accept(this);

                return lastValue;
            });
            
        }

        public Double Visit(FunctionCallNode functionCallNode)
        {
            var functionDefinition = ResolveFunctionCall(functionCallNode);

            return WithNewScopeDo(scope =>
            {
                for (int i = 0; i < functionCallNode.Arguments.Count; i++)
                {
                    var name = functionDefinition.Arguments[i];
                    var value = functionCallNode.Arguments[i].Accept(this);

                    scope.Define(name, value);
                }

                return functionDefinition.Body.Accept(this);
            });
        }

        private FunctionDefinitionNode ResolveFunctionCall(FunctionCallNode functionCallNode)
        {
            var functionName = functionCallNode.Name;
            var functionDefinition = CurrentFunctionScope.Resolve(functionName);

            if (functionDefinition.Arguments.Count != functionCallNode.Arguments.Count)
                throw new BadFunctionCall(functionName, functionDefinition.Arguments.Count, functionCallNode.Arguments.Count);

            return functionDefinition;
        }

        private Double WithNewScopeDo(Func<Scope<Double>, Double> action)
        {
            var newScope = new Scope<Double>(CurrentVariableScope);

            variableScopes.Push(newScope);
            Double result = action(newScope);
            variableScopes.Pop();

            return result;
        }
    }
}