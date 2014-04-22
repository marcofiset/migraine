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
            return WithNewScopesDo((_, functionScope) =>
            {
                var parser = new SymbolTableParser();
                blockNode.Accept(parser);

                foreach (var func in parser.functions.Keys)
                {
                    functionScope.Define(func, parser.functions[func]);
                }

                Double lastValue = 0;

                foreach (var exp in blockNode.Expressions)
                    lastValue = exp.Accept(this);

                return lastValue;
            });
            
        }

        public Double Visit(FunctionCallNode functionCallNode)
        {
            var functionDefinition = ResolveFunctionCall(functionCallNode);

            return WithNewScopesDo((variableScope, functionScope) =>
            {
                for (int i = 0; i < functionCallNode.Arguments.Count; i++)
                {
                    var name = functionDefinition.Arguments[i];
                    var value = functionCallNode.Arguments[i].Accept(this);

                    variableScope.Define(name, value);
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

        private Double WithNewScopesDo(Func<Scope<Double>, Scope<FunctionDefinitionNode>, Double> action)
        {
            var newVariableScope = new Scope<Double>(CurrentVariableScope);
            var newFunctionScope = new Scope<FunctionDefinitionNode>(CurrentFunctionScope);

            variableScopes.Push(newVariableScope);
            functionScopes.Push(newFunctionScope);

            Double result = action(newVariableScope, newFunctionScope);

            variableScopes.Pop();
            functionScopes.Pop();

            return result;
        }

        public Double Visit(IfStatementNode ifStatementNode)
        {
            var conditionValue = ifStatementNode.Condition.Accept(this);

            if (Convert.ToBoolean(conditionValue))
                return ifStatementNode.Body.Accept(this);

            return 0;
        }

        public double Visit(ConditionNode conditionNode)
        {
            var leftValue = conditionNode.LeftOperand.Accept(this);

            if (conditionNode.Operator == null)
                return leftValue;

            var rightValue = conditionNode.RightOperand.Accept(this);
            Boolean result;

            switch (conditionNode.Operator)
            {
                case "==":
                    result = leftValue == rightValue;
                    break;

                case ">=":
                    result = leftValue >= rightValue;
                    break;

                case "<=":
                    result = leftValue <= rightValue;
                    break;

                case ">":
                    result = leftValue > rightValue;
                    break;

                case "<":
                    result = leftValue < rightValue;
                    break;

                default:
                    throw new Exception("Undefined operator :" + conditionNode.Operator);
            }

            return result ? 1 : 0;
        }
    }
}