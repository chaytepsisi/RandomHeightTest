using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomHeightTest
{
    internal class Program
    {
        static readonly Random random = new Random();
        static readonly bool SHOW_INFO=true;
        static readonly bool NO_INFO = false;
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

        static string GenerateBiasedString(int length, double bias)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int setSize = 1;
            while(bias<1)
            {
                setSize *= 10;
                bias *= 10;
            }    

            for (int i = 0; i < length; i++)
            {
                if (random.Next(setSize) < setSize/2+bias)
                    stringBuilder.Append(1);
                else stringBuilder.Append(0);
            }
            return stringBuilder.ToString();
        }

        static void Main(string[] args)
        {
            int sequenceLength = 128;
            int numberOfTrials = 100000;
            TestRandomSequence(sequenceLength, numberOfTrials);
            TestBiasedSequence(sequenceLength, numberOfTrials,0.01);
            GetBiasedMissRate(sequenceLength, numberOfTrials, 0.001);
        }
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
        static void GetBiasedMissRate(int sequenceLength, int numberOfTrials, double bias)
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
                if (heightTest.Evaluate(NO_INFO) >= 0.01)
                    miss++;
            }

            Console.WriteLine("Bias: " + bias);
            Console.WriteLine("Misses: " + miss + "/" + 100);
        }
    }
}
