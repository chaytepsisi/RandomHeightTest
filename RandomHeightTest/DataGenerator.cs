using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomHeightTest
{
    internal class DataGenerator
    {
        static readonly Random random = new Random();

        /// <summary>
        /// Generates a random sequence of given length
        /// </summary>
        /// <param name="length"> Length of the sequence</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length, int numberOfSequences=1)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int j = 0; j < numberOfSequences; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    if (random.Next(2) == 0)
                        stringBuilder.Append(1);
                    else stringBuilder.Append(0);
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Generates a biased string
        /// </summary>
        /// <param name="length"> Length of the sequence</param>
        /// <param name="bias"> Bias of the sequence</param>
        /// <returns></returns>
        public static string GenerateBiasedString(int length, double bias)
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

        public static string GenerateSpecialBiasedSequence(int length, int numberOfSequences, int fixedLength)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string prefix = "";
            string suffix = "";
            for (int i = 0; i < fixedLength; i++)
            {
                prefix += "1";
                suffix += "0";
            }
            for (int j = 0; j < numberOfSequences; j++)
            {
                stringBuilder.Append(prefix);
                for (int i = prefix.Length; i < length - suffix.Length; i++)
                {
                    if (random.Next(2) == 0)
                        stringBuilder.Append("1");
                    else stringBuilder.Append("0");
                }
                stringBuilder.Append(suffix);
            }
            return stringBuilder.ToString();
        }

        public static void GenerateSpecialBiasedSequence(int length, int numberOfSequences, int fixedLength, string fileName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string prefix = "";
            string suffix = "";
            StreamWriter writer = new StreamWriter(fileName);
            for (int i = 0; i < fixedLength; i++)
            {
                prefix += "1";
                suffix += "0";
            }
            for (int j = 0; j < numberOfSequences; j++)
            {
                writer.Write(prefix);
                for (int i = prefix.Length; i < length - suffix.Length; i++)
                {
                    if (random.Next(2) == 0)
                        writer.Write("1");
                    else writer.Write("0");
                }
                writer.Write(suffix);
            }
            writer.Flush();
            writer.Close();
        }
    }
}
