using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace RandomHeightTest
{
    internal class Testing
    {
        static readonly bool SHOW_INFO = true;
        static readonly bool NO_INFO = false;
        static readonly int BYTE_LENGTH = 8;

        public const int GENERATOR_AES = 1;
        public const int GENERATOR_SHA256 = 2;
        public const int GENERATOR_SHA512 = 3;
        public const int GENERATOR_MD5 = 4;
        public const int FILE_TESTER = 5;

        /// <summary>
        /// Tests an ASCII file of 1s and 0s containing concatenated sequences of given length
        /// </summary>
        /// <param name="sequenceLength"></param>
        /// <param name="numberOfTrials"></param>
        /// <param name="filePath"></param>
        public static Result TestFile(int sequenceLength, int numberOfTrials, string filePath, BackgroundWorker bgw = null)
        {
            StreamReader reader = new StreamReader(filePath);
            HeightTest heightTest = new HeightTest(sequenceLength);
            string sequence;
            char[] buffer;
            for (int j = 0; j < numberOfTrials; j++)
            {
                buffer = new char[sequenceLength];
                reader.ReadBlock(buffer, 0, sequenceLength);
                sequence = new string(buffer);
                heightTest.Test(sequence);

                if (bgw != null && j % (numberOfTrials / 100) == 0)
                {
                    bgw.ReportProgress((int)(j / (numberOfTrials / 100.0)));
                }
            }
            var pVal = heightTest.Evaluate(SHOW_INFO);

            Console.WriteLine("P-Value: " + pVal);
            return new Result()
            {
                PValue = pVal,
                Text = heightTest.ResultString
            };
        }

        /// <summary>
        /// Tests randomly generated sequences given the length and the number of sequences
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        public static void TestRandomSequence(int sequenceLength, int numberOfTrials)
        {
            string str;
            HeightTest heightTest = new HeightTest(sequenceLength);

            for (int j = 0; j < numberOfTrials; j++)
            {
                str = DataGenerator.GenerateRandomString(sequenceLength);
                heightTest.Test(str);
            }

            Console.WriteLine("P-Value: " + heightTest.Evaluate(SHOW_INFO));
        }
        /// <summary>
        /// Tests randomly generated sequences which are generated with the given bias
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        /// <param name="bias"> Bias of the sequences</param>
        public static void TestBiasedSequence(int sequenceLength, int numberOfTrials, double bias)
        {
            string str;
            HeightTest heightTest = new HeightTest(sequenceLength);

            for (int j = 0; j < numberOfTrials; j++)
            {
                str = DataGenerator.GenerateBiasedString(sequenceLength, bias);
                heightTest.Test(str);
            }
            Console.WriteLine("P-Value: " + heightTest.Evaluate(SHOW_INFO));
        }
        /// <summary>
        /// Computes the number of times the test is unable to correctly distinguish
        /// a biased sequence from a randomly generated sequence.
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        /// <param name="bias"> Bias of the sequences</param>
        /// <param name="criticalValue"> Critical value to determine if the sequence fails the test</param>
        public static void GetBiasedMissRate(int sequenceLength, int numberOfTrials, double bias, double criticalValue = 0.01, BackgroundWorker bgw = null)
        {
            string str;
            int miss = 0;

            for (int i = 0; i < 100; i++)
            {
                HeightTest heightTest = new HeightTest(sequenceLength);

                for (int j = 0; j < numberOfTrials; j++)
                {
                    str = DataGenerator.GenerateBiasedString(sequenceLength, bias);
                    heightTest.Test(str);
                }
                var pVal = heightTest.Evaluate(NO_INFO);
                if (pVal >= criticalValue)
                    miss++;

                if (bgw != null)
                {
                    Console.WriteLine(pVal);
                    bgw.ReportProgress((i + 1));
                }
            }

            Console.WriteLine("Bias: " + bias);
            Console.WriteLine("Misses: " + miss + "/" + 100);
        }

        /// <summary>
        /// Reads from a binary file and test the content as "sequenceLength"-bit sequences
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sequenceLength"></param>
        public static Result ReadFromBinaryFile(string filePath, int sequenceLength)
        {
            var allBytes = System.IO.File.ReadAllBytes(filePath);
            var sequenceBytes = sequenceLength / BYTE_LENGTH;
            var numberOfSequences = allBytes.Length / sequenceBytes;

            int byteIndex = 0;
            StringBuilder str;

            HeightTest heightTest = new HeightTest(sequenceLength);
            for (int i = 0; i < numberOfSequences; i++)
            {
                str = new StringBuilder();
                for (int j = 0; j < sequenceBytes; j++)
                {
                    str.Append(Convert.ToString(allBytes[byteIndex++], 2).PadLeft(8, '0'));
                }
                heightTest.Test(str.ToString());
            }
            var pVal = heightTest.Evaluate(SHOW_INFO);
            Console.WriteLine("P-Value: " + heightTest.ResultString);
            return new Result()
            {
                PValue = pVal,
                Text = heightTest.ResultString
            };
        }
        /// <summary>
        /// Generates numberOfTrials sequence of length "sequenceLength" from SHA2-256 or SHA2-512 
        /// In order to maintain reproducibility a chain of hash values are computed as follows:
        ///     First, the initial value 0x00,0x00,...,0x00 of length 128 bits is hashed and the hash 
        ///     value is taken as a sequence. Then, this hash value is hashed to get the next sequence 
        ///     and so on. 
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        /// <param name="generator"> SHA2-256 or SHA2-512</param>
        public static Result TestSha2(int sequenceLength, int numberOfTrials, int generator, BackgroundWorker bgw = null)
        {
            byte[] initialBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            HashData hashData = new HashData(initialBytes);
            HeightTest heightTest = new HeightTest(sequenceLength);
            string str = "";
            for (int j = 0; j < numberOfTrials; j++)
            {
                if (generator == GENERATOR_SHA512)
                    str = hashData.GenerateSHA512Data(sequenceLength);
                else if (generator == GENERATOR_SHA256)
                    str = hashData.GenerateSHA256Data(sequenceLength);
                else if (generator == GENERATOR_MD5)
                    str = hashData.GenerateMD5Data(sequenceLength);

                heightTest.Test(str);
                if (bgw != null && j % (numberOfTrials / 100) == 0)
                {
                    bgw.ReportProgress((int)(j / (numberOfTrials / 100.0)));
                }
            }
            var pVal = heightTest.Evaluate(SHOW_INFO);
            Console.WriteLine("P-Value: " + heightTest.ResultString);
            return new Result()
            {
                PValue = pVal,
                Text = heightTest.ResultString
            };
        }

        /// <summary>
        /// At each iteration i, i is encrypted with full zero key and the output is stored. 
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        /// <param name="generator"> AES-128</param>
        public static Result TestAES128(int sequenceLength, int numberOfTrials, int generator, BackgroundWorker bgw = null)
        {
            EncryptedData encData = new EncryptedData();
            string fileName = "aes_" + sequenceLength + ".txt";
            if (generator == GENERATOR_AES)
            {
                encData.GenerateAESDataFile(sequenceLength, numberOfTrials, fileName, 0, bgw);
            }
            var result = TestFile(sequenceLength, numberOfTrials, fileName, bgw);
            Console.WriteLine("P-Value: " + result.PValue.ToString());
            return result;
        }
    }
}