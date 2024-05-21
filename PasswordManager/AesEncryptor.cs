using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    internal class AesEncryptor
    {
        public static byte[] generateKey()
        {
            var aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.GenerateKey();
            return aesAlg.Key;
        }

        public static byte[] generateIV()
        {
            var aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.GenerateIV();
            return aesAlg.IV;
        }

        public static void encryptFile(string file, byte[] key)
        {
            if (!File.Exists(file)) throw(new FileNotFoundException("File-to-encrypt does not exist."));

            var aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = key;
            
            byte[] chunkIn = new byte[4096];
            byte[] chunkOut = new byte[4096];
            var cTransfrom = aesAlg.CreateEncryptor(aesAlg.Key, new byte[16]);


            using (Stream input = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (Stream output = File.Open(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                int current = input.Read(chunkIn, 0, chunkIn.Length);
                while (current > 0)
                {
                    if (current < chunkIn.Length) chuknIn = chunkIn.Take(current);
                    chunkOut = encryptData(chunkIn, cTransfrom);
                    current = input.Read(chunkIn, 0, chunkIn.Length);
                    output.Write(chunkOut, 0, chunkOut.Length);
                }
            }
        }

        public static void decryptFile(string file, byte[] key)
        {
            if (!File.Exists(file)) throw (new FileNotFoundException("File-to-encrypt does not exist."));

            var aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = key;

            byte[] chunkIn = new byte[4096];
            byte[] chunkOut;
            var cTransfrom = aesAlg.CreateDecryptor(aesAlg.Key, new byte[16]);


            using (Stream input = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (Stream output = File.Open(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                int current = input.Read(chunkIn, 0, chunkIn.Length);
                while (current > 0)
                {
                    if (current < chunkIn.Length) chunkIn.Take(current);
                    chunkOut = encryptData(chunkIn, cTransfrom);
                    current = input.Read(chunkIn, 0, chunkIn.Length);
                    output.Write(chunkOut, 0, current);
                }
            }
        }

        private static byte[] encryptData(byte[] data, ICryptoTransform cTransform) 
        {
            using(var ms = new MemoryStream())
            using(var cryptoStream = new CryptoStream(ms, cTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }
    }
}
