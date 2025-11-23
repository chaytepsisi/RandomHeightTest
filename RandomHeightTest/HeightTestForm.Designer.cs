namespace RandomHeightTest
{
    partial class HeightTestForm
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
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.GeneratorsCbx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NumberOfSequencesTbx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TestButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ResultTbx = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PvalueTbx = new System.Windows.Forms.TextBox();
            this.testBgw = new System.ComponentModel.BackgroundWorker();
            this.SequenceLengthCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Location = new System.Drawing.Point(208, 3);
            this.SelectFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(100, 28);
            this.SelectFileButton.TabIndex = 0;
            this.SelectFileButton.Text = "Select File";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // GeneratorsCbx
            // 
            this.GeneratorsCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GeneratorsCbx.FormattingEnabled = true;
            this.GeneratorsCbx.Location = new System.Drawing.Point(95, 5);
            this.GeneratorsCbx.Margin = new System.Windows.Forms.Padding(4);
            this.GeneratorsCbx.Name = "GeneratorsCbx";
            this.GeneratorsCbx.Size = new System.Drawing.Size(100, 24);
            this.GeneratorsCbx.TabIndex = 1;
            this.GeneratorsCbx.SelectedIndexChanged += new System.EventHandler(this.GeneratorsCbx_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Generator";
            // 
            // NumberOfSequencesTbx
            // 
            this.NumberOfSequencesTbx.Location = new System.Drawing.Point(95, 65);
            this.NumberOfSequencesTbx.Name = "NumberOfSequencesTbx";
            this.NumberOfSequencesTbx.Size = new System.Drawing.Size(100, 22);
            this.NumberOfSequencesTbx.TabIndex = 4;
            this.NumberOfSequencesTbx.Text = "1000000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Bit Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "#Sequences";
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(208, 62);
            this.TestButton.Margin = new System.Windows.Forms.Padding(4);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(100, 28);
            this.TestButton.TabIndex = 8;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 121);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(301, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // ResultTbx
            // 
            this.ResultTbx.Location = new System.Drawing.Point(7, 150);
            this.ResultTbx.Name = "ResultTbx";
            this.ResultTbx.ReadOnly = true;
            this.ResultTbx.Size = new System.Drawing.Size(301, 302);
            this.ResultTbx.TabIndex = 10;
            this.ResultTbx.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 96);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "P-Value:";
            // 
            // PvalueTbx
            // 
            this.PvalueTbx.Location = new System.Drawing.Point(95, 93);
            this.PvalueTbx.Name = "PvalueTbx";
            this.PvalueTbx.ReadOnly = true;
            this.PvalueTbx.Size = new System.Drawing.Size(100, 22);
            this.PvalueTbx.TabIndex = 12;
            // 
            // testBgw
            // 
            this.testBgw.WorkerReportsProgress = true;
            this.testBgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TestBgw_DoWork);
            this.testBgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TestBgw_ProgressChanged);
            this.testBgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TestBgw_RunWorkerCompleted);
            // 
            // SequenceLengthCbx
            // 
            this.SequenceLengthCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SequenceLengthCbx.FormattingEnabled = true;
            this.SequenceLengthCbx.Location = new System.Drawing.Point(95, 35);
            this.SequenceLengthCbx.Margin = new System.Windows.Forms.Padding(4);
            this.SequenceLengthCbx.Name = "SequenceLengthCbx";
            this.SequenceLengthCbx.Size = new System.Drawing.Size(100, 24);
            this.SequenceLengthCbx.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "label5";
            // 
            // HeightTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 457);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SequenceLengthCbx);
            this.Controls.Add(this.PvalueTbx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ResultTbx);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NumberOfSequencesTbx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GeneratorsCbx);
            this.Controls.Add(this.SelectFileButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "HeightTestForm";
            this.Text = "HeightTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.ComboBox GeneratorsCbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NumberOfSequencesTbx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox ResultTbx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PvalueTbx;
        private System.ComponentModel.BackgroundWorker testBgw;
        private System.Windows.Forms.ComboBox SequenceLengthCbx;
        private System.Windows.Forms.Label label5;
    }
}