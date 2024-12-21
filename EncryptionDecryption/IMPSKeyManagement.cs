using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using MIIPL.Common;

namespace DALC
{
    public class MaximusAESEncryption
    {

        public static string Encrypt(string plainText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;


            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        public static string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }

        public static string EncryptString(string plainText, string SsmMasterKey)
        {
            string Pin = string.Empty;
            byte[] Key = Utils.ASCIIToByteArray(SsmMasterKey);

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            using (Aes _aesAlg = Aes.Create())
            {
                _aesAlg.Key = Key;
                ICryptoTransform _encryptor = _aesAlg.CreateEncryptor(_aesAlg.Key, Utils.ASCIIToByteArray("0000000000000000"));

                using (MemoryStream _memoryStream = new MemoryStream())
                {
                    _memoryStream.Write(Utils.ASCIIToByteArray(""), 0, 0);
                    using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, _encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter _streamWriter = new StreamWriter(_cryptoStream))
                        {
                            _streamWriter.Write(plainText);
                        }
                        return Utils.ByteArrayToHex(_memoryStream.ToArray());
                    }
                }
            }
        }

        public static string EncryptStringBT(string plainText, string KeyString)
        {
            string Pin = string.Empty;
            byte[] Key = Utils.ASCIIToByteArray(KeyString);

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            using (Aes _aesAlg = Aes.Create())
            {
                _aesAlg.Mode = CipherMode.ECB;
                _aesAlg.Padding = PaddingMode.PKCS7;
                _aesAlg.FeedbackSize = 128;
                _aesAlg.KeySize = 128;
                _aesAlg.Key = Key;
                _aesAlg.IV = Utils.ASCIIToByteArray("0000000000000000");
                ICryptoTransform _encryptor = _aesAlg.CreateEncryptor(_aesAlg.Key, Utils.ASCIIToByteArray("0000000000000000"));

                using (MemoryStream _memoryStream = new MemoryStream())
                {
                    _memoryStream.Write(Utils.ASCIIToByteArray(""), 0, 0);
                    using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, _encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter _streamWriter = new StreamWriter(_cryptoStream))
                        {
                            _streamWriter.Write(plainText);
                        }
                        return Convert.ToBase64String(_memoryStream.ToArray());
                    }
                }
            }
        }

        public static string EncryptBT(string clearText,string key)
        {
            string EncryptionKey = key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] IV = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
                Rfc2898DeriveBytes pdb = new
                    Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = ASCIIEncoding.UTF8.GetBytes(key);
                encryptor.IV = IV;
                encryptor.Mode = CipherMode.CBC;
                encryptor.KeySize = 128;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptString(string CyperText, string SsmMasterKey)
        {
            string Pin = string.Empty;
            byte[] Key = Utils.ASCIIToByteArray(SsmMasterKey);
            if (CyperText == null || CyperText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = Key;
                rijAlg.IV = Utils.ASCIIToByteArray("0000000000000000");
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    using (var msDecrypt = new MemoryStream(Utils.HexToByteArray(CyperText)))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    return Pin;
                }
            }
        }

        //public static string DecryptString(string CyperText)
        //{
        //    IBTG.MIIPLPINManager _pinManager = new MIIPLPINManager();
        //    _pinManager.RefreshKeys("");
        //    byte[] key = Utils.ASCIIToByteArray("34CC9A188F2B50DFC8D3DDFA8B8BD856");
        //    byte[] cipherText = Utils.HexToByteArray(cipherText1);
        //    byte[] iv = Utils.ASCIIToByteArray("0000000000000000");

        //    if (cipherText == null || cipherText.Length <= 0)
        //    {
        //        throw new ArgumentNullException("cipherText");
        //    }
        //    if (key == null || key.Length <= 0)
        //    {
        //        throw new ArgumentNullException("key");
        //    }
        //    if (iv == null || iv.Length <= 0)
        //    {
        //        throw new ArgumentNullException("key");
        //    }

        //    // Declare the string used to hold  
        //    // the decrypted text.  
        //    string plaintext = null;

        //    // Create an RijndaelManaged object  
        //    // with the specified key and IV.  
        //    using (var rijAlg = new RijndaelManaged())
        //    {
        //        //Settings  
        //        rijAlg.Mode = CipherMode.CBC;
        //        rijAlg.Padding = PaddingMode.PKCS7;
        //        rijAlg.FeedbackSize = 128;

        //        rijAlg.Key = key;
        //        rijAlg.IV = Utils.ASCIIToByteArray("0000000000000000");

        //        // Create a decrytor to perform the stream transform.  
        //        var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

        //        try
        //        {
        //            // Create the streams used for decryption.  
        //            using (var msDecrypt = new MemoryStream(cipherText))
        //            {
        //                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //                {

        //                    using (var srDecrypt = new StreamReader(csDecrypt))
        //                    {
        //                        // Read the decrypted bytes from the decrypting stream  
        //                        // and place them in a string.  
        //                        plaintext = srDecrypt.ReadToEnd();

        //                    }

        //                }
        //            }
        //        }
        //        catch
        //        {
        //            plaintext = "keyError";
        //        }
        //    }

        //    return plaintext;
        //}
    }
}
