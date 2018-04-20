using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CSLERP.Encription
{
    class AESEncription
    {
        private const string AesIV256 = @"CellcommSolution";
        private const string AesKey256 = @"Cellcomm Solutios Ltd @NV office";

        public string Encrypt256(string text)
        {
            string encyptedString = "";
            try
            {
                // AesCryptoServiceProvider
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(AesIV256);
                aes.Key = Encoding.UTF8.GetBytes(AesKey256);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                
                // Convert string to byte array
                byte[] src = Encoding.Unicode.GetBytes(text);

                // encryption
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                    // Convert byte array to Base64 strings
                    encyptedString= Convert.ToBase64String(dest);
                }
            }
            catch (Exception)
            {
                encyptedString = "";
            }
            return encyptedString;
        }
        public string Decrypt256(string text)
        {
            string decyptedString = "";
            try
            {
                // AesCryptoServiceProvider
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(AesIV256);
                aes.Key = Encoding.UTF8.GetBytes(AesKey256);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Convert Base64 strings to byte array
                byte[] src = System.Convert.FromBase64String(text);

                // decryption
                using (ICryptoTransform decrypt = aes.CreateDecryptor())
                {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    decyptedString= Encoding.Unicode.GetString(dest);
                }
            }
            catch (Exception)
            {
                decyptedString = "";
            }
            return decyptedString;
        }
    }
}
