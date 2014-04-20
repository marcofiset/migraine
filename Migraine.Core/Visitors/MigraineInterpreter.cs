﻿using Migraine.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            double lastValue = 0;
            scopes.Push(new Scope(CurrentScope));

            foreach (var exp in blockNode.Expressions)
                lastValue = exp.Accept(this);

            scopes.Pop();

            return lastValue;
        }

        public Double Visit(FunctionCallNode functionCallNode)
        {
            var functionName = functionCallNode.Name;

            if (!functions.ContainsKey(functionName))
                throw new Exception(String.Format("Function {0} is undefined.", functionName));

            var functionDefinition = functions[functionName];

            if (functionDefinition.Arguments.Count != functionCallNode.Arguments.Count)
                throw new Exception(String.Format("Wrong number of arguments passed to function {0}", functionName));

            var functionScope = new Scope(CurrentScope);

            for (int i = 0; i < functionCallNode.Arguments.Count; i++)
            {
                var name = functionDefinition.Arguments[i];
                var value = functionCallNode.Arguments[i].Accept(this);

                functionScope.DefineVariable(name, value);
            }

            scopes.Push(functionScope);
            double result = functionDefinition.Body.Accept(this);
            scopes.Pop();

            return result;
        }
    }
}