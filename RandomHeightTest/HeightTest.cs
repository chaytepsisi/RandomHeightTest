using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RandomHeightTest
{
    internal class HeightTest
    {


        readonly int[] observed;
        int[] limits;
        double[] probs;
        int numberOfSamples;
        public int SequenceLength { get; set; }

        public HeightTest(int sequenceLength)
        {
            SequenceLength = sequenceLength;
            observed = new int[8];
            numberOfSamples = 0;
            GetBinLimits();
            GetBinProbs();
        }
        public void Test(string sequence)
        {
            numberOfSamples++;
            int h = GetHeight(sequence);
            for (int i = 0; i < limits.Length; i++)
            {
                if (h < limits[i])
                {
                    observed[i]++;
                    break;
                }
            }
        }

        public void Test(int[] sequence)
        {
            numberOfSamples++;
            int h = GetHeight(sequence);
            for (int i = 0; i < limits.Length; i++)
            {
                if (h < limits[i])
                {
                    observed[i]++;
                    break;
                }
            }
        }

        int GetHeight(int[] sequence)
        {
            int maxHeight = int.MinValue;
            int y = 0;
            for (int i = 0; i < sequence.Length; i++)
            {
                y += (1 - 2 * sequence[i]);
                if (y > maxHeight)
                    maxHeight = y;
            }
            return maxHeight;
        }

        int GetHeight(string sequence)
        {
            int maxHeight = int.MinValue;
            int y = 0;
            for (int i = 0; i < sequence.Length; i++)
            {
                if (sequence[i] == '1')
                    y--;
                else y++;

                if (y > maxHeight)
                    maxHeight = y;
            }
            return maxHeight;
        }

        void GetBinLimits()
        {
            switch (SequenceLength)
            {
                case 128:
                    limits = Stats.binLimits_128;
                    break;
                case 256:
                    limits = Stats.binLimits_256;
                    break;
                case 1024:
                    limits = Stats.binLimits_1024;
                    break;
                case 4096:
                    limits = Stats.binLimits_4096;
                    break;
                default:
                    Console.WriteLine("Sequence length not supported!!");
                    limits = null;
                    break;
            }
        }

        void GetBinProbs()
        {
            switch (SequenceLength)
            {
                case 128:
                    probs = Stats.binProbs_128;
                    break;
                case 256:
                    probs = Stats.binProbs_256;
                    break;
                case 1024:
                    probs = Stats.binProbs_1024;
                    break;
                case 4096:
                    probs = Stats.binProbs_4096;
                    break;
                default:
                    Console.WriteLine("Sequence length not supported!!");
                    probs = null;
                    break;
            }
        }

        public double Evaluate(bool verbose=false)
        {
            if (probs == null)
                return 0.0;

            double sum = 0.0;
            for (int i = 0; i < probs.Length; i++)
            {
                var expected = probs[i] * numberOfSamples;
                sum += ((expected - observed[i]) * (expected - observed[i])) / expected;
            }
            if(verbose)
                PrintObservedValues(sum);
          
            return ChiSquare.Pvalue(sum, probs.Length - 1);
        }

        void PrintObservedValues(double chi2Value)
        {
            Console.WriteLine("Sequence Length: " + SequenceLength);
            Console.WriteLine("Number of Samples: " + numberOfSamples);
            Console.WriteLine("- - - - - - - - - - - - - - - - -");
            Console.WriteLine("       Expctd    Obsrvd");
            for (int i = 0; i < observed.Length; i++)
                Console.WriteLine("Bin-" + (i + 1) + ": " + observed[i] + "\t" + probs[i] * numberOfSamples);

            Console.WriteLine();

            Console.WriteLine("Degree Of Freedom: " + (probs.Length - 1));
            Console.WriteLine("X2 Value: " + chi2Value);
        }
    }
}
