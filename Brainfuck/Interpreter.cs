using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brainfuck
{
    public class Interpreter
    {
        private Int32 currentProgramStringIndex;
        private Stack<Int32> loopIndexes;

        public String ProgramString { get; private set; }
        public Int32 PointerPosition { get; private set; }
        public List<Int32> MemoryCells { get; private set; }

        public Int32 CurrentCellValue
        {
            get { return MemoryCells[PointerPosition]; }
        }

        private IInputProvider inputProvider;
        private IOutputProvider outputDestination;

        private Dictionary<char, Action> actions;

        /// <summary>
        /// Constructor with which you can supply a single input/output source
        /// </summary>
        /// <param name="inputOutput">The input/output provider</param>
        public Interpreter(IInputOutputProvider inputOutput, String program = "")
            : this(inputOutput, inputOutput, program)
        {

        }

        /// <summary>
        /// Constructor with which you can supply the input and the output provider independently
        /// </summary>
        /// <param name="input">The input provider</param>
        /// <param name="output">The output provider</param>
        public Interpreter(IInputProvider input, IOutputProvider output, String program = "") 
        {
            if (input == null) throw new ArgumentNullException("input");
            if (output == null) throw new ArgumentNullException("output");

            this.inputProvider = input;
            this.outputDestination = output;

            InitializeActionList(input, output);

            ValidateSourceFile(program);

            ProgramString = program;
            PointerPosition = 0;
            loopIndexes = new Stack<Int32>();
            MemoryCells = new List<Int32>();
            MemoryCells.Add(0);
        }

        private void InitializeActionList(IInputProvider input, IOutputProvider output)
        {
            actions = new Dictionary<char, Action>();

            //Pointer manipulation
            actions.Add('>', () =>
            {
                PointerPosition++;

                //Allocate new memory cells as we go
                if (MemoryCells.Count <= PointerPosition)
                    MemoryCells.Add(0);
            });

            actions.Add('<', () =>
            {
                PointerPosition--;

                if (PointerPosition < 0)
                    PointerPosition = 0;
            });

            //Current cell manipulation
            actions.Add('+', () => MemoryCells[PointerPosition]++);
            actions.Add('-', () => MemoryCells[PointerPosition]--);

            //Input and output
            actions.Add(',', () => MemoryCells[PointerPosition] = Convert.ToInt32(input.Get()));
            actions.Add('.', () => output.Write(MemoryCells[PointerPosition].ToString()));

            actions.Add('[', () =>
            {
                loopIndexes.Push(currentProgramStringIndex);

                //Enter the loop if the current memory cell is different than zero
                if (MemoryCells[PointerPosition] != 0)
                    return;

                //Else we skip until the end of that loop
                do
                {
                    currentProgramStringIndex++;

                    //Stack-based logic in case we encounter any inner loops
                    if (ProgramString[currentProgramStringIndex] == '[')
                    {
                        loopIndexes.Push(currentProgramStringIndex);
                    }
                    else if (ProgramString[currentProgramStringIndex] == ']')
                    {
                        loopIndexes.Pop();
                    }
                //FIXME: Potential bug here if we are already in a loop
                } while (loopIndexes.Count > 0);
            });

            actions.Add(']', () =>
            {
                //Go back to the start of the loop
                if (MemoryCells[PointerPosition] != 0)
                    currentProgramStringIndex = loopIndexes.Peek();
                else
                    loopIndexes.Pop();
            });
        }

        /// <summary>
        /// Validates a given brainfuck source file to make sure it is a valid program
        /// </summary>
        /// <param name="program">A string containing the program to validate</param>
        private void ValidateSourceFile(string program)
        {
            if (program == null) throw new ArgumentNullException("program");

            var brackets = new Stack<char>();

            for (int i = 0; i < program.Length; i++)
            {
                char action = program[i];

                if (!actions.ContainsKey(action))
                    throw new Exception("Invalid character in source file");

                if (action == '[')
                {
                    brackets.Push(action);
                }
                else if (action == ']')
                {
                    if (brackets.Count == 0)
                        throw new Exception("Opening and ending bracket count does not match");

                    brackets.Pop();
                }
            }

            if (brackets.Count != 0)
                throw new Exception("Opening and ending bracket cound does not match");
        }

        /// <summary>
        /// Executes the given Brainfuck program
        /// </summary>
        /// <param name="program">The program to execute (Optional, as it may have been supplied in the constructor)</param>
        public void Execute(String program = "")
        {
            ValidateSourceFile(program);

            if (program != "")
                ProgramString = program;

            for (currentProgramStringIndex = 0; currentProgramStringIndex < ProgramString.Length; currentProgramStringIndex++)
            {
                char currentChar = ProgramString[currentProgramStringIndex];
                actions[currentChar]();
            }
        }
    }
}
