using NUnit.ConsoleRunner;
using System;
using System.Linq;

namespace ExpressionEvaluator.Tests2
{
    class Program
    {
        static void Main(string[] args)
        {
            String newArgs = "";
            args.ToList().ForEach(s => newArgs += " " + s);

            Runner.Main(newArgs.Split(";".ToCharArray()));
        }
    }
}
