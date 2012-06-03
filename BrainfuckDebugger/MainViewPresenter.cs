using BrainfuckDebugger.Interfaces;
using System.IO;
using System;

namespace BrainfuckDebugger
{
    /// <summary>
    /// Object that handles the business logic of the MainView
    /// </summary>
    public class MainViewPresenter
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
            var interpreter = new Brainfuck.Interpreter();
            interpreter.Execute(view.BrainfuckProgram);
        }

        /// <summary>
        /// Save the current program to a file
        /// </summary>
        public void SaveToFile()
        {
            string fileName = view.ChooseFile(FileAction.Save);
            if (String.IsNullOrEmpty(fileName)) return;

            var writer = new StreamWriter(fileName);
            
            writer.Flush();
            writer.Write(view.BrainfuckProgram);

            writer.Close();
            writer.Dispose();
        }
    }
}
