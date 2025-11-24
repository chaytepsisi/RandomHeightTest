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
        readonly string[] filetypes= new string[] { "Select", "ASCII", "Binary", "Hex" };
        Result pValue;

        int selectedGenerator = -1;
        int sequenceLength = 128;
        int numberOfSequences = 1000000;
        string filePath = string.Empty;
        readonly Stopwatch stopwatch = new Stopwatch();
        int fileType = 0;
        public HeightTestForm()
        {
            InitializeComponent();
            GeneratorsCbx.DataSource = generators;
            SequenceLengthCbx.DataSource = bitLengths;
            FileTypeCbox.DataSource = filetypes;    
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
                if (fileType == 0)
                {
                    MessageBox.Show("Please select file type.");
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
            //TestButton.Enabled = false;
            panel1.Enabled = false;
        }
        private void GeneratorsCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GeneratorsCbx.SelectedIndex == generators.Length - 1)
            {
                SelectFileButton.Enabled = true;
                FileTypeCbox.Enabled = true;
            }
            else
            {
                filePath = "";
                SelectFileButton.Enabled = false;
                FileTypeCbox.Enabled = false;
            }
        }
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    ResultTbx.Text = filePath;
                    FileTypeCbox.Enabled = true;
                }
            }
        }
        private void TestBgw_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (selectedGenerator)
            {
                case Testing.GENERATOR_AES:
                    pValue = Testing.TestAES128(sequenceLength, numberOfSequences, Testing.GENERATOR_AES, testBgw);
                    break;
                case Testing.GENERATOR_SHA256:
                    pValue = Testing.TestHash(sequenceLength, numberOfSequences, Testing.GENERATOR_SHA256, testBgw);
                    break;
                case Testing.GENERATOR_SHA512:
                    pValue = Testing.TestHash(sequenceLength, numberOfSequences, Testing.GENERATOR_SHA512, testBgw);
                    break;
                case Testing.GENERATOR_MD5:
                    pValue = Testing.TestHash(sequenceLength, numberOfSequences, Testing.GENERATOR_MD5, testBgw);
                    break;
                case Testing.FILE_TESTER:
                    switch (fileType)
                    {
                        case 1: //ASCII
                            pValue = Testing.TestFile(sequenceLength, numberOfSequences, filePath,testBgw);
                            break;
                        case 2: //Binary
                            pValue = Testing.ReadFromBinaryFile(filePath, sequenceLength);
                            break;
                        case 3: //Hex
                            pValue=Testing.TestHexFile(sequenceLength, numberOfSequences, filePath, testBgw);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        private void TestBgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();
            progressBar1.Value = 100;
            label5.Text = "%100";
            PvalueTbx.Text = pValue.PValue.ToString();
            if(pValue.PValue<0.01)
                PvalueTbx.BackColor = Color.Red;
            else PvalueTbx.BackColor = Color.LightGreen;
            ResultTbx.Text=pValue.Text;
            ResultTbx.Text+= Environment.NewLine + "Time taken: " + (stopwatch.ElapsedMilliseconds / 1000.0) + "s";
            //TestButton.Enabled = true;
            panel1.Enabled = true;
        }
        private void TestBgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label5.Text = "%" + e.ProgressPercentage;
        }

        private void FileTypeCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fileType=FileTypeCbox.SelectedIndex;
        }

        private void PvalueTbx_MouseClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(pValue.PValue.ToString());
        }
    }
}