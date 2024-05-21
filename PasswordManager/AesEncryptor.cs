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

        public static void encryptFile(string file, byte[] key)
        {
            if (!File.Exists(file)) throw(new FileNotFoundException("File-to-encrypt does not exist."));
            var stream = File.Open(file, FileMode.Open);

            var aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = key;
            
            byte[] chunk = new byte[4096];
            int bytesRead;
            var cTransfrom = aesAlg.CreateEncryptor(aesAlg.Key, null);

            var reader = new BinaryReader(stream);
            var writer = new BinaryWriter(stream);

            while (((chunk = reader.ReadBytes(chunk.Length)).Length > 0))
            {
                chunk = encryptData(chunk, cTransfrom);
            }

            reader.Read(chunk);
            chunk = encryptData(chunk, cTransfrom);
            writer.Write(chunk);
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
