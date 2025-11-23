using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace RandomHeightTest
{
    internal class EncryptedData
    {
        const int AES_BITLENGTH = 128;

        byte[] Ptext;
        readonly byte[] Key;
        readonly byte[] IV;

        public EncryptedData()
        {
            Key = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            IV = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }
        public EncryptedData(byte[] keyData, byte[] iv)
        {
            Key = keyData;
            IV = iv;
        }
        public string GenerateAESData(long counter, int sequenceLength)
        {
            var tempPtext = BitConverter.GetBytes(counter);
            Ptext=new byte[16];
            for (int i = 0; i < tempPtext.Length; i++)
            {
                Ptext[i] = tempPtext[i];
            }

            int numberOfEncryptions = (sequenceLength + AES_BITLENGTH - 1) / AES_BITLENGTH;

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < numberOfEncryptions; i++)
            {
                AES_Encrypt();
                stringBuilder.Append(ConvertByteToBinaryString(Ptext));
            }
            return stringBuilder.ToString().Substring(0, sequenceLength);
        }
        public void GenerateAESDataFile(int sequenceLength, int numberOfSequences, string filePath, long counter=0, BackgroundWorker bgw=null)
        {
            int numberOfEncryptions = (sequenceLength + AES_BITLENGTH - 1) / AES_BITLENGTH;
            StreamWriter writer = new StreamWriter(filePath);

            byte[] tempPtext;

            for (int j = 0; j < numberOfSequences; j++)
            {
                tempPtext = BitConverter.GetBytes(counter + j);
                Ptext = new byte[16];

                for (int i = 0; i < tempPtext.Length; i++)
                {
                    Ptext[i] = tempPtext[i];
                }

                for (int i = 0; i < numberOfEncryptions; i++)
                {
                    AES_Encrypt();
                    writer.Write(ConvertByteToBinaryString(Ptext));
                }
                if (bgw != null && j % (numberOfSequences / 100) == 0)
                {
                    bgw.ReportProgress((int)(j / (numberOfSequences / 100.0)));
                    writer.Flush();
                }
            }

            writer.Flush();
            writer.Close();
        }
        void AES_Encrypt()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.Key = Key;
                aes.IV = IV;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.Zeros;


                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(Ptext, 0, Ptext.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    Ptext = ms.ToArray();
                }
            }
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
