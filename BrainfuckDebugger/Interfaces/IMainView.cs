using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
