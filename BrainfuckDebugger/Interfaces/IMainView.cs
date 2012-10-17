using Brainfuck;
namespace BrainfuckDebugger.Interfaces
{
    /// <summary>
    /// Interface reprensenting the MainView
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// Asks the user for a filename
        /// </summary>
        /// <param name="action">The action we'll do with the file</param>
        /// <returns>The path to the file</returns>
        string ChooseFile(FileAction action);

        /// <summary>
        /// The string of the Brainfuck program
        /// </summary>
        string BrainfuckProgram { get; set; }

        /// <summary>
        /// Gets input for the Brainfuck program
        /// </summary>
        /// <returns></returns>
        string GetInput();

        /// <summary>
        /// Clears the output of the view
        /// </summary>
        void ClearOutput();

        /// <summary>
        /// Write the output of the Brainfuck program
        /// </summary>
        /// <param name="value">The value of the output</param>
        void WriteToOutput(string value);
    }
}
