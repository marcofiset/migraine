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

            //Pointer manipulation
            actions.Add('>', () => 
            { 
                pointerPosition++;

                //Allocate new memory cells as we go
                if (memory.Count <= pointerPosition)
                    memory.Add(0);
            });

            actions.Add('<', () => 
            {
                pointerPosition--;

                if (pointerPosition < 0)
                    pointerPosition = 0;
            });

            //Current cell manipulation
            actions.Add('+', () => memory[pointerPosition]++);
            actions.Add('-', () => memory[pointerPosition]--);

            //Input and output
            actions.Add(',', () => memory[pointerPosition] = Convert.ToInt32(input.Get()));
            actions.Add('.', () => output.Write(memory[pointerPosition].ToString()));

            actions.Add('[', () =>
            {
                loopIndexes.Push(currentIndex);

                //Enter the loop if the current memory cell is different than zero
                if (memory[pointerPosition] != 0)
                    return;

                //Else we skip until the end of that loop
                do
                {
                    currentIndex++;

                    //Stack-based logic in case we encounter any inner loops
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
                //Go back to the start of the loop
                if (memory[pointerPosition] != 0)
                    currentIndex = loopIndexes.Peek();
                else
                    loopIndexes.Pop();
            });
        }

        /// <summary>
        /// Executes the given Brainfuck program
        /// </summary>
        /// <param name="program">A string containing the brainfuck program to execute</param>
        public void Execute(string program)
        {
            if (program == null) throw new ArgumentNullException("program");
            ValidateSourceFile(program);

            programString = program;
            pointerPosition = 0;
            loopIndexes = new Stack<int>();
            memory = new List<int?>();
            memory.Add(0);

            for (currentIndex = 0; currentIndex < program.Length; currentIndex++)
            {
                char currentChar = programString[currentIndex];
                actions[currentChar]();
            }
        }

        /// <summary>
        /// Validates a given brainfuck source file to make sure it is a valid program
        /// </summary>
        /// <param name="program">A string containing the program to validate</param>
        private void ValidateSourceFile(string program)
        {
            if (program == null) throw new ArgumentNullException("program");

            for (int i = 0; i < program.Length; i++)
            {
                if (!actions.ContainsKey(program[i]))
                    throw new Exception("Invalid character in source file");
            }
        }
    }
}
