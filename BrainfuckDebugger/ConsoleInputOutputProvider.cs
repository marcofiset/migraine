using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brainfuck;

namespace BrainfuckDebugger
{
    public class ConsoleInputOutputProvider : IInputOutputProvider
    {
        public string Get()
        {
            return Console.ReadLine();
        }

        public void Write(string value)
        {
            Console.WriteLine(value);
        }
    }
}
