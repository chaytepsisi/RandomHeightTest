using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomHeightTest
{
    internal class HashData
    {
        const int SHA512_BITLENGTH = 512;
        const int SHA256_BITLENGTH = 256;
        byte[] Data;

        public HashData(byte[] hashData)
        {
            Data = hashData;
        }

        public string GenerateSHA512Data(int sequenceLength)
        {
            int numberOfHashComputations = sequenceLength / SHA512_BITLENGTH + 1;

            StringBuilder stringBuilder = new StringBuilder();
            using (SHA512 sha = SHA512.Create())
            {
                for (int i = 0; i < numberOfHashComputations; i++)
                {
                    Data = sha.ComputeHash(Data);
                    stringBuilder.Append(ConvertByteToBinaryString(Data));
                }
            }
            return stringBuilder.ToString().Substring(0, sequenceLength);
        }

        public string GenerateSHA256Data(int sequenceLength)
        {
            int numberOfHashComputations = sequenceLength / SHA256_BITLENGTH + 1;

            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 sha = SHA256.Create())
            {
                for (int i = 0; i < numberOfHashComputations; i++)
                {
                    Data = sha.ComputeHash(Data);
                    stringBuilder.Append(ConvertByteToBinaryString(Data));
                }
            }
            return stringBuilder.ToString().Substring(0, sequenceLength);
        }


        string ConvertByteToBinaryString(byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
                sb.Append(Convert.ToString(value[i], 2).PadLeft(8, '0'));
            return sb.ToString();
        }

    }
}