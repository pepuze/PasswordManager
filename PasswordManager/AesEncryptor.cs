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

        public static void encryptFile(string file, byte[] key)
        {
            if (!File.Exists(file)) throw(new FileNotFoundException("File-to-encrypt does not exist."));

            string parentDirectory = Path.GetDirectoryName(file);
            string fileNameEncrypyted = Path.GetFileNameWithoutExtension(file) + ".enc";
            string fileEncrypted = Path.Combine(parentDirectory, fileNameEncrypyted);
            var streamOut = File.Create(fileEncrypted);

            aesGcm = new AesGcm(key);

            var aesAlg = Aes.Create();
            aesAlg.KeySize = 256;
            aesAlg.Key = key;
            aesAlg.IV 
            byte[] chunk = new byte[4096];
            int bytesRead;

            Stream streamIn = File.OpenRead(file);
            while((bytesRead = streamIn.Read(chunk, 0, chunk.Length)) > 0)
            { 
                
            }

            streamIn.Close();
            streamOut.Flush();
            streamOut.Close();
        }
    }
}
