namespace BrainfuckDebugger
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.programTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loadFromFileButton = new System.Windows.Forms.Button();
            this.executeButton = new System.Windows.Forms.Button();
            this.saveToFileButton = new System.Windows.Forms.Button();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.promptForInputRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.preSupplyInputRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.memoryGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoryGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // programTextBox
            // 
            this.programTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.programTextBox.Location = new System.Drawing.Point(12, 34);
            this.programTextBox.Multiline = true;
            this.programTextBox.Name = "programTextBox";
            this.programTextBox.Size = new System.Drawing.Size(502, 108);
            this.programTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter your Brainfuck program here :";
            // 
            // loadFromFileButton
            // 
            this.loadFromFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFromFileButton.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadFromFileButton.Location = new System.Drawing.Point(520, 34);
            this.loadFromFileButton.Name = "loadFromFileButton";
            this.loadFromFileButton.Size = new System.Drawing.Size(142, 32);
            this.loadFromFileButton.TabIndex = 2;
            this.loadFromFileButton.Text = "Load from a file...";
            this.loadFromFileButton.UseVisualStyleBackColor = true;
            this.loadFromFileButton.Click += new System.EventHandler(this.loadFromFileButton_Click);
            // 
            // executeButton
            // 
            this.executeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.executeButton.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeButton.Location = new System.Drawing.Point(520, 110);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(142, 32);
            this.executeButton.TabIndex = 3;
            this.executeButton.Text = "Execute!";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // saveToFileButton
            // 
            this.saveToFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveToFileButton.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveToFileButton.Location = new System.Drawing.Point(520, 72);
            this.saveToFileButton.Name = "saveToFileButton";
            this.saveToFileButton.Size = new System.Drawing.Size(142, 32);
            this.saveToFileButton.TabIndex = 4;
            this.saveToFileButton.Text = "Save to a file...";
            this.saveToFileButton.UseVisualStyleBackColor = true;
            this.saveToFileButton.Click += new System.EventHandler(this.saveToFileButton_Click);
            // 
            // inputTextBox
            // 
            this.inputTextBox.Enabled = false;
            this.inputTextBox.Location = new System.Drawing.Point(6, 81);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(304, 25);
            this.inputTextBox.TabIndex = 5;
            // 
            // promptForInputRadioButton
            // 
            this.promptForInputRadioButton.AutoSize = true;
            this.promptForInputRadioButton.Checked = true;
            this.promptForInputRadioButton.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.promptForInputRadioButton.Location = new System.Drawing.Point(6, 25);
            this.promptForInputRadioButton.Name = "promptForInputRadioButton";
            this.promptForInputRadioButton.Size = new System.Drawing.Size(186, 22);
            this.promptForInputRadioButton.TabIndex = 6;
            this.promptForInputRadioButton.TabStop = true;
            this.promptForInputRadioButton.Text = "Prompt for input as needed";
            this.promptForInputRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.preSupplyInputRadioButton);
            this.groupBox1.Controls.Add(this.promptForInputRadioButton);
            this.groupBox1.Controls.Add(this.inputTextBox);
            this.groupBox1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 121);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Program Input";
            // 
            // preSupplyInputRadioButton
            // 
            this.preSupplyInputRadioButton.AutoSize = true;
            this.preSupplyInputRadioButton.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preSupplyInputRadioButton.Location = new System.Drawing.Point(6, 53);
            this.preSupplyInputRadioButton.Name = "preSupplyInputRadioButton";
            this.preSupplyInputRadioButton.Size = new System.Drawing.Size(291, 22);
            this.preSupplyInputRadioButton.TabIndex = 7;
            this.preSupplyInputRadioButton.Text = "Pre-supply input (separated by semi-colons) :";
            this.preSupplyInputRadioButton.UseVisualStyleBackColor = true;
            this.preSupplyInputRadioButton.CheckedChanged += new System.EventHandler(this.preSupplyInputRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.outputTextBox);
            this.groupBox2.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(342, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 121);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Program Output";
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(6, 25);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.Size = new System.Drawing.Size(304, 81);
            this.outputTextBox.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.memoryGridView);
            this.groupBox3.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 275);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(650, 275);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Program Memory";
            // 
            // memoryGridView
            // 
            this.memoryGridView.AllowUserToAddRows = false;
            this.memoryGridView.AllowUserToDeleteRows = false;
            this.memoryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.memoryGridView.Location = new System.Drawing.Point(10, 25);
            this.memoryGridView.Name = "memoryGridView";
            this.memoryGridView.ReadOnly = true;
            this.memoryGridView.Size = new System.Drawing.Size(630, 231);
            this.memoryGridView.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 562);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.saveToFileButton);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.loadFromFileButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.programTextBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Brainfuck Debugger";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoryGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox programTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadFromFileButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.Button saveToFileButton;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.RadioButton promptForInputRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton preSupplyInputRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView memoryGridView;
    }
}

