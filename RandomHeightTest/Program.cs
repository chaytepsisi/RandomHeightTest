using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RandomHeightTest
{
    internal class Program
    {


        static BackgroundWorker backgroundWorker;
        static void Main(string[] args)
        {
            //backgroundWorker = new BackgroundWorker();
            //backgroundWorker.WorkerReportsProgress = true;
            //backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;


            int sequenceLength = 1024;
            int numberOfTrials = 1000000;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DataGenerator.GenerateSpecialBiasedSequence(sequenceLength, numberOfTrials, 2, sequenceLength + ".txt");
            //Testing.TestRandomSequence(sequenceLength, numberOfTrials);
            //Testing.TestBiasedSequence(sequenceLength, numberOfTrials,0.01);
            //Testing.Testing.GetBiasedMissRate(sequenceLength, numberOfTrials, 0.0005, 0.01, backgroundWorker);
            //Testing.ReadFromBinaryFile("pi", 4096);
            //Testing.TestSha2(sequenceLength, numberOfTrials, GENERATOR_SHA256);
            //Testing.TestAES128(sequenceLength, numberOfTrials, GENERATOR_AES);
            Testing.TestFile2(sequenceLength, numberOfTrials, sequenceLength + ".txt");

            stopwatch.Stop();
            Console.WriteLine("Total Time: " + (stopwatch.ElapsedMilliseconds / 1000.0) + "s");

        }

        static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine( e.ProgressPercentage + "%");
        }
        
    }
}
