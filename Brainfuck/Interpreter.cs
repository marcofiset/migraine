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

        private IInputProvider input;
        private IOutputProvider output;

        private Dictionary<char, Action> actions;

        /// <summary>
        /// Constructor with which you can supply a single input/output source
        /// </summary>
        /// <param name="inputOutput">The input/output provider</param>
        public Interpreter(IInputOutputProvider inputOutput)
            : this(inputOutput, inputOutput)
        {

        }

        /// <summary>
        /// Constructor with which you can supply the input and the output provider independently
        /// </summary>
        /// <param name="input">The input provider</param>
        /// <param name="output">The output provider</param>
        public Interpreter(IInputProvider input, IOutputProvider output) 
        {
            if (input == null) throw new ArgumentNullException("input");
            if (output == null) throw new ArgumentNullException("output");

            this.input = input;
            this.output = output;

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

            actions.Add(',', () => memory[pointerPosition] = Convert.ToInt32(input.Get()));
            actions.Add('.', () => output.Write(memory[pointerPosition].ToString()));

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
                    currentIndex = loopIndexes.Peek();
                else
                    loopIndexes.Pop();
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
                char currentChar = programString[currentIndex];
                if (actions.ContainsKey(currentChar))
                    actions[currentChar]();
            }
        }
    }
}
