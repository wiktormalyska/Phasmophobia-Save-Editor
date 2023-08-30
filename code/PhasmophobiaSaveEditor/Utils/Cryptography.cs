using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhasmoSaveEditor
{
    internal class Cryptography
    {
        string password = "t36gref9u84y7f43g";

        public byte[] decryptFile(byte[] encryptedSaveBytes)
        {
            byte[] decryptedSaveBytes = null;

            try
            {
                byte[] iv = new byte[16];
                Array.Copy(encryptedSaveBytes, iv, 16);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = DeriveKeyFromPassword(password, iv, 100, 16); // 16 bytes = 128 bits
                    aesAlg.IV = iv;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                    using (MemoryStream msDecrypt = new MemoryStream(encryptedSaveBytes, 16, encryptedSaveBytes.Length - 16))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (MemoryStream decryptedMemoryStream = new MemoryStream())
                    {
                        csDecrypt.CopyTo(decryptedMemoryStream);
                        decryptedSaveBytes = decryptedMemoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed decrypting the save file\nWrong decryption password?");
                Console.WriteLine(ex.Message);
                return null;
            }

            return decryptedSaveBytes;
        }



        public void encryptFile(string decryptedSave, string encryptedSave) {

            byte[] encryptedSaveBytes = Encoding.UTF8.GetBytes(encryptedSave);
            byte[] decryptedSaveBytes = Encoding.UTF8.GetBytes(decryptedSave);

            try
            {
                byte[] iv;
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    iv = new byte[16];
                    rng.GetBytes(iv);
                }

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = DeriveKeyFromPassword(password, iv, 100, 16); // 16 bytes = 128 bits
                    aesAlg.IV = iv;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                    using (MemoryStream encryptedMemoryStream = new MemoryStream())
                    using (CryptoStream csEncrypt = new CryptoStream(encryptedMemoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(decryptedSaveBytes, 0, decryptedSave.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    encryptedSave = Encoding.UTF8.GetString(encryptedSaveBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed encrypting the save file\nInternal error");
                Console.WriteLine(ex.Message);
            }
        }

        static byte[] CombineArrays(byte[] array1, byte[] array2)
        {
            byte[] combined = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, combined, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, combined, array1.Length, array2.Length);
            return combined;
        }
        static byte[] DeriveKeyFromPassword(string password, byte[] salt, int iterations, int keySize)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(keySize);
            }
        }

    }
}
