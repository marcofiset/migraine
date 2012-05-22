using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.ConsoleRunner;

namespace ExpressionEvaluator.TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string newArgs = "";
			//TODO : Use String.Join();
            args.ToList().ForEach(s => newArgs += " " + s);

            Runner.Main(newArgs.Split(";".ToCharArray()));
        }
    }
}
