using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomHeightTest
{
    internal class Program
    {
        static readonly Random random = new Random();
        static readonly bool SHOW_INFO = true;
        static readonly bool NO_INFO = false;
        static readonly int BYTE_LENGTH = 8;

        static readonly int GENERATOR_SHA256 = 0;
        static readonly int GENERATOR_SHA512 = 1;
        static readonly int GENERATOR_AES = 2;

        /// <summary>
        /// Generates a random sequence of given length
        /// </summary>
        /// <param name="length"> Length of the sequence</param>
        /// <returns></returns>
        static string GenerateRandomString(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                if (random.Next(2) == 0)
                    stringBuilder.Append(1);
                else stringBuilder.Append(0);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Generates a biased string
        /// </summary>
        /// <param name="length"> Length of the sequence</param>
        /// <param name="bias"> Bias of the sequence</param>
        /// <returns></returns>
        static string GenerateBiasedString(int length, double bias)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int setSize = 1;
            while (bias < 1)
            {
                setSize *= 10;
                bias *= 10;
            }

            for (int i = 0; i < length; i++)
            {
                if (random.Next(setSize) < setSize / 2 + bias)
                    stringBuilder.Append(1);
                else stringBuilder.Append(0);
            }
            return stringBuilder.ToString();
        }
        static void Main(string[] args)
        {
            int sequenceLength = 4096;
            int numberOfTrials = 1000000;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //TestRandomSequence(sequenceLength, numberOfTrials);
            //TestBiasedSequence(sequenceLength, numberOfTrials,0.01);
            //GetBiasedMissRate(sequenceLength, numberOfTrials, 0.001);
            //ReadFromBinaryFile("pi", 4096);
            TestSha2(sequenceLength, numberOfTrials, GENERATOR_SHA256);
            //TestAES128(sequenceLength, numberOfTrials, GENERATOR_AES);

            stopwatch.Stop();
            Console.WriteLine("Total Time: " + (stopwatch.ElapsedMilliseconds / 1000.0) + "s");

        }
        /// <summary>
        /// Tests randomly generated sequences given the length and the number of sequences
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        static void TestRandomSequence(int sequenceLength, int numberOfTrials)
        {
            string str;
            HeightTest heightTest = new HeightTest(sequenceLength);

            for (int j = 0; j < numberOfTrials; j++)
            {
                str = GenerateRandomString(sequenceLength);
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
        static void TestBiasedSequence(int sequenceLength, int numberOfTrials, double bias)
        {
            string str;
            HeightTest heightTest = new HeightTest(sequenceLength);

            for (int j = 0; j < numberOfTrials; j++)
            {
                str = GenerateBiasedString(sequenceLength, bias);
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
        static void GetBiasedMissRate(int sequenceLength, int numberOfTrials, double bias, double criticalValue = 0.01)
        {
            string str;
            int miss = 0;

            for (int i = 0; i < 100; i++)
            {
                HeightTest heightTest = new HeightTest(sequenceLength);

                for (int j = 0; j < numberOfTrials; j++)
                {
                    str = GenerateBiasedString(sequenceLength, bias);
                    heightTest.Test(str);
                }
                if (heightTest.Evaluate(NO_INFO) >= criticalValue)
                    miss++;
            }

            Console.WriteLine("Bias: " + bias);
            Console.WriteLine("Misses: " + miss + "/" + 100);
        }

        /// <summary>
        /// Reads from a binary file and test the content as "sequenceLength"-bit sequences
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sequenceLength"></param>
        static void ReadFromBinaryFile(string filePath, int sequenceLength)
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
            Console.WriteLine("P-Value: " + heightTest.Evaluate(SHOW_INFO));
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
        static void TestSha2(int sequenceLength, int numberOfTrials, int generator)
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
                heightTest.Test(str);
            }

            Console.WriteLine("P-Value: " + heightTest.Evaluate(SHOW_INFO));
        }

        /// <summary>
        /// At each iteration i, i is encrypted with full zero key and the output is stored. 
        /// </summary>
        /// <param name="sequenceLength"> Length of each tested sequence</param>
        /// <param name="numberOfTrials"> Number of tested sequences</param>
        /// <param name="generator"> AES-128</param>
        static void TestAES128(int sequenceLength, int numberOfTrials, int generator)
        {
            HeightTest heightTest = new HeightTest(sequenceLength);
            EncryptedData encData = new EncryptedData();
            string str = "";
            for (int j = 0; j < numberOfTrials; j++)
            {
                if (generator == GENERATOR_AES)
                    str = encData.GenerateAESData(j, sequenceLength);
               
                heightTest.Test(str);
            }

            Console.WriteLine("P-Value: " + heightTest.Evaluate(SHOW_INFO));
        }
    }
}
