using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrainfuckDebugger
{
    public partial class InputForm : Form
    {
        /// <summary>
        /// The value entered in the textbox
        /// </summary>
        public String Value 
        {
            get { return valueTextBox.Text; }
        }

        public InputForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Value))
                this.Close();
        }
    }
}
