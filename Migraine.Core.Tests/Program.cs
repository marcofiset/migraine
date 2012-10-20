using NUnit.ConsoleRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migraine.Core.Tests
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
