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
            string str;
            int sequenceLength = 128;
            int numberOfTrials = 100000;
            int negative = 0;
            for (int i = 0; i < 100; i++)
            {
                HeightTest heightTest = new HeightTest(sequenceLength);

                for (int j = 0; j < numberOfTrials; j++)
                {
                    //str = GenerateRandomString(sequenceLength);
                    str = GenerateBiasedString(sequenceLength, 0.001);
                    heightTest.Test(str);
                }
                if(heightTest.Evaluate(false)>=0.01)
                    negative++;
                //Console.WriteLine("P-Value: " + heightTest.Evaluate(false));
            }

            Console.WriteLine(negative);
        }
    }
}
