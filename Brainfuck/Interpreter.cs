using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brainfuck
{
    public class Interpreter
    {
        private string programString;
        private int currentIndex;
        private int pointerPosition;
        private Stack<int> loopIndexes;
        private List<int?> memory;

        private Dictionary<char, Action> actions;

        public Interpreter() 
        {
            actions = new Dictionary<char, Action>();

            actions.Add('>', () => 
            { 
                pointerPosition++;

                if (memory.Count <= pointerPosition)
                    memory.Add(0);
            });

            actions.Add('<', () => 
            {
                pointerPosition--;

                if (pointerPosition < 0)
                    pointerPosition = memory.Count - 1;
            });

            actions.Add('+', () => memory[pointerPosition]++);
            actions.Add('-', () => memory[pointerPosition]--);

            actions.Add(',', () => memory[pointerPosition] = Convert.ToInt32(Console.ReadLine()));
            actions.Add('.', () => Console.WriteLine(memory[pointerPosition]));

            actions.Add('[', () =>
            {
                loopIndexes.Push(currentIndex);

                if (memory[pointerPosition] != 0)
                    return;

                do
                {
                    currentIndex++;

                    if (programString[currentIndex] == '[')
                    {
                        loopIndexes.Push(currentIndex);
                    }
                    else if (programString[currentIndex] == ']')
                    {
                        loopIndexes.Pop();
                    }

                } while (loopIndexes.Count > 0);
            });

            actions.Add(']', () =>
            {
                if (memory[pointerPosition] != 0)
                    currentIndex = loopIndexes.Pop();
            });
        }

        public void Execute(string program)
        {
            if (program == null) throw new ArgumentNullException("program");

            programString = program;
            pointerPosition = 0;
            loopIndexes = new Stack<int>();
            memory = new List<int?>();
            memory.Add(0);

            for (currentIndex = 0; currentIndex < program.Length; currentIndex++)
            {
                actions[programString[currentIndex]]();
            }
        }
    }
}
