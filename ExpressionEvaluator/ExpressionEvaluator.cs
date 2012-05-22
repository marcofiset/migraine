using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionEvaluator
{
    public class ExpressionEvaluator
    {
        private List<TokenDefinition> tokenDefinitions;
        private Dictionary<string, Operator> operators;
        private Lexer lexer;

        public ExpressionEvaluator()
        {
            InitializeOperators();
            InitializeTokenDefinitions();

            lexer = new Lexer(tokenDefinitions);
        }

        private void InitializeOperators()
        {
            operators = new Dictionary<string, Operator>();
			
            operators.Add("(", new Operator("(", -1, Associativity.Ignore, 0));
            operators.Add(")", new Operator(")", -1, Associativity.Ignore, 0));
			
            operators.Add("~", new Operator("~", 4, Associativity.Right, 1));
            operators.Add("^", new Operator("^", 3, Associativity.Right, 2));
            operators.Add("*", new Operator("*", 2, Associativity.Left, 2));
            operators.Add("/", new Operator("/", 2, Associativity.Left, 2));
            operators.Add("+", new Operator("+", 1, Associativity.Left, 2));
            operators.Add("-", new Operator("-", 1, Associativity.Left, 2));
        }

        private void InitializeTokenDefinitions()
        {
            tokenDefinitions = new List<TokenDefinition>();

            if (operators == null) InitializeOperators();

            string operatorString = "";

            foreach (string op in operators.Keys)
            {
                operatorString += op;
            }

            string operatorPattern = String.Format("[{0}]", Regex.Escape(operatorString));

            RegexMatchEvaluator operatorMatcher = new RegexMatchEvaluator(operatorPattern);
            RegexMatchEvaluator whiteSpaceMatcher = new RegexMatchEvaluator(@"[\s]+");
            RegexMatchEvaluator numberMatcher = new RegexMatchEvaluator(@"(\d)+(\.[\d])*");
            RegexMatchEvaluator identifierMatch = new RegexMatchEvaluator(@"[A-Za-z0-9_]+");

            tokenDefinitions.Add(new TokenDefinition(operatorMatcher, TokenType.Operator));
            tokenDefinitions.Add(new TokenDefinition(whiteSpaceMatcher, TokenType.Whitespace));
            tokenDefinitions.Add(new TokenDefinition(numberMatcher, TokenType.Number));
            tokenDefinitions.Add(new TokenDefinition(identifierMatch, TokenType.Identifier));
        }

        public Double Evaluate(string expression)
        {
            IEnumerable<Token> tokens = lexer.Tokenize(expression);

            Stack<String> operatorStack = new Stack<String>();
            Queue<String> outputQueue = new Queue<String>();

            Action<String> HandleOperator = (currentOp) =>
            {
                String topOperator = operatorStack.Count > 0 ? operatorStack.Peek() : null;

                if (currentOp == "(")
                {
                    operatorStack.Push(currentOp);
                    return;
                }

                if (currentOp == ")")
                {
                    try
                    {
                        while (operatorStack.Peek() != "(")
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }

                        operatorStack.Pop();
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("Mismatched parenthesis");
                    }
                }

                while (topOperator != null
                       && (operators[currentOp].Associativity == Associativity.Left && operators[currentOp].Precedence <= operators[topOperator].Precedence
                       || operators[currentOp].Associativity == Associativity.Right && operators[currentOp].Precedence < operators[topOperator].Precedence))
                {
                    outputQueue.Enqueue(topOperator);

                    operatorStack.Pop();
                    topOperator = operatorStack.Count > 0 ? operatorStack.Peek() : null;
                }

                operatorStack.Push(currentOp);
            };

            Token previousToken = null;

            foreach (Token token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                    case TokenType.Identifier:
                        outputQueue.Enqueue(token.Value);
                        break;

                    case TokenType.Operator:
                        bool isMinus = token.Value == "-";
                        bool isUnary = previousToken == null || previousToken.Type != TokenType.Number;

                        if (isMinus && isUnary)
                            HandleOperator("~");
                        else
                            HandleOperator(token.Value);
                        break;

                    case TokenType.Whitespace:
                    default:
                        break;
                }

                if (token.Type != TokenType.Whitespace)
                    previousToken = token;
            }

            while ((operatorStack.Count > 0 ? operatorStack.Peek() : null) != null)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }
			
            var valueStack = new Stack<String>();

            while (outputQueue.Count > 0)
            {
                var value = outputQueue.Dequeue();
                while (!operators.ContainsKey(value))
                {
                    valueStack.Push(value);

                    if (outputQueue.Count > 0)
                        value = outputQueue.Dequeue();
                }

                var args = new double[operators[value].ArgumentsCount];
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    args[i] = Convert.ToDouble(valueStack.Pop());
                }

                switch (value)
                {
                    case "+":
                        valueStack.Push(Convert.ToString(args[0] + args[1]));
                        break;
                    case "-":
                        valueStack.Push(Convert.ToString(args[0] - args[1]));
                        break;
                    case "*":
                        valueStack.Push(Convert.ToString(args[0] * args[1]));
                        break;
                    case "/":
                        valueStack.Push(Convert.ToString(args[0] / args[1]));
                        break;
                    case "^":
                        valueStack.Push(Convert.ToString(Math.Pow(args[0], args[1])));
                        break;
                    case "~":
                        valueStack.Push(Convert.ToString(-args[0]));
                        break;
                }
            }

            return Convert.ToDouble(valueStack.Pop());
        }
    }
}