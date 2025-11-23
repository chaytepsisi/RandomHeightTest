using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomHeightTest
{
    public partial class HeightTestForm : Form
    {
        readonly string[] generators = new string[] { "Select", "AES-128", "SHA2-256", "SHA2-512","MD5", "File" };
        readonly string[] bitLengths= new string[] { "Select", "128", "256", "1024", "4096" };
        Result pValue;

        int selectedGenerator = -1;
        int sequenceLength = 128;
        int numberOfSequences = 1000000;
        string filePath = string.Empty;
        readonly bool isBinaryFile = false;
        readonly Stopwatch stopwatch = new Stopwatch();

        public HeightTestForm()
        {
            InitializeComponent();
            GeneratorsCbx.DataSource = generators;
            SequenceLengthCbx.DataSource = bitLengths;
        }
        private void TestButton_Click(object sender, EventArgs e)
        {
            try
            {
                selectedGenerator = GeneratorsCbx.SelectedIndex;
            }catch
            {
                MessageBox.Show("Please select a generator.");
                return;
            }

            if(selectedGenerator == Testing.FILE_TESTER)
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Please select a valid file path.");
                    return;
                }
            }
            try
            {
                sequenceLength = int.Parse(SequenceLengthCbx.SelectedItem.ToString());
            }
            catch { 
              MessageBox.Show("Please select a valid sequence length.");
                return;
            }
            numberOfSequences = int.Parse(NumberOfSequencesTbx.Text);
            PvalueTbx.BackColor=SystemColors.Control;
            PvalueTbx.Clear();
            ResultTbx.Clear();
            stopwatch.Restart();
            testBgw.RunWorkerAsync();
            TestButton.Enabled = false;
        }
        private void GeneratorsCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GeneratorsCbx.SelectedIndex == generators.Length - 1)
            {
                //FilePathTbx.Enabled = true;
                SelectFileButton.Enabled = true;
            }
            else
            {
                //FilePathTbx.Enabled = false;
                filePath = "";
                SelectFileButton.Enabled = false;
            }
        }
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    //FilePathTbx.Text = filePath;
                    ResultTbx.Text = filePath;
                }
            }
        }
        private void TestBgw_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (selectedGenerator)
            {
                case Testing.GENERATOR_AES:
                    pValue = Testing.TestAES128(sequenceLength, numberOfSequences, selectedGenerator, testBgw);
                    break;
                case Testing.GENERATOR_SHA256:
                    pValue = Testing.TestSha2(sequenceLength, numberOfSequences, selectedGenerator, testBgw);
                    break;
                case Testing.GENERATOR_SHA512:
                    pValue = Testing.TestSha2(sequenceLength, numberOfSequences, selectedGenerator, testBgw);
                    break;
                case Testing.FILE_TESTER:
                    if (isBinaryFile)
                        pValue = Testing.ReadFromBinaryFile(filePath, sequenceLength);
                    else
                        pValue = Testing.TestFile(sequenceLength, numberOfSequences, filePath);
                    break;
                default:
                    break;
            }
        }
        private void TestBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();
            PvalueTbx.Text = pValue.PValue.ToString();
            if(pValue.PValue<0.01)
                PvalueTbx.BackColor = Color.Red;
            else PvalueTbx.BackColor = Color.LightGreen;
            ResultTbx.Text=pValue.Text;
            ResultTbx.Text+= Environment.NewLine + "Time taken: " + (stopwatch.ElapsedMilliseconds / 1000.0) + "s";
            TestButton.Enabled = true;
        }
        private void TestBgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label5.Text = "%" + e.ProgressPercentage;
        }
    }
}