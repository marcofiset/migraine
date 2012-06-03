using System.Windows.Forms;
using BrainfuckDebugger.Interfaces;
using System;

namespace BrainfuckDebugger
{
    public partial class MainForm : Form, IMainView
    {
        private MainViewPresenter presenter;

        /// <summary>
        /// The string of the Brainfuck program
        /// </summary>
        public string BrainfuckProgram 
        {
            get { return programTextBox.Text; }
            set { programTextBox.Text = value; }
        }

        public MainForm()
        {
            InitializeComponent();

            this.presenter = new MainViewPresenter(this);
        }

        /// <summary>
        /// Asks the user to choose a file through an OpenFileDialog
        /// </summary>
        /// <returns>The file name chosen by the user</returns>
        public string ChooseFile(FileAction action)
        {
            FileDialog fileDialog;

            switch (action)
            {
                case FileAction.Save:
                    fileDialog = new SaveFileDialog();
                    break;

                case FileAction.Open:
                    fileDialog = new OpenFileDialog();
                    break;

                default:
                    throw new ArgumentException("The argument should be a valid FileAction enum value.", "action");
            }

            fileDialog.ShowDialog();
            
            return fileDialog.FileName;
        }

        private void loadFromFileButton_Click(object sender, System.EventArgs e)
        {
            presenter.LoadFromFile();
        }

        private void executeButton_Click(object sender, System.EventArgs e)
        {
            presenter.ExecuteProgram();
        }

        private void saveToFileButton_Click(object sender, System.EventArgs e)
        {
            presenter.SaveToFile();
        }
    }
}
