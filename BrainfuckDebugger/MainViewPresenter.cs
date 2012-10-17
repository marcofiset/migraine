using BrainfuckDebugger.Interfaces;
using System.IO;
using System;
using Brainfuck;

namespace BrainfuckDebugger
{
    /// <summary>
    /// Object that handles the business logic of the MainView
    /// </summary>
    public class MainViewPresenter : IInputOutputProvider
    {
        private IMainView view;

        /// <summary>
        /// Constructor which takes an IMainView as a parameter
        /// </summary>
        /// <param name="view">The view</param>
        public MainViewPresenter(IMainView view)
        {
            this.view = view;
        }

        /// <summary>
        /// Loads a brainfuck program from a file
        /// </summary>
        public void LoadFromFile()
        {
            string fileName = view.ChooseFile(FileAction.Open);
            if (String.IsNullOrEmpty(fileName)) return;

            var reader = new StreamReader(fileName);
            string fileContent = reader.ReadToEnd();

            view.BrainfuckProgram = fileContent;
        }

        /// <summary>
        /// Executes the Brainfuck program
        /// </summary>
        public void ExecuteProgram()
        {
            view.ClearOutput();

            var interpreter = new Brainfuck.Interpreter(this);
            interpreter.Execute(view.BrainfuckProgram);
        }

        /// <summary>
        /// Save the current program to a file
        /// </summary>
        public void SaveToFile()
        {
            string fileName = view.ChooseFile(FileAction.Save);
            if (String.IsNullOrEmpty(fileName)) return;

            using (var writer = new StreamWriter(fileName))
            {
                writer.Flush();
                writer.Write(view.BrainfuckProgram);

                writer.Close();
            }
        }

        string IInputProvider.Get()
        {
            return view.GetInput();
        }

        void IOutputProvider.Write(string value)
        {
            view.WriteToOutput(value);
        }
    }
}
