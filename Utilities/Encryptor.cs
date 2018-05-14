using System;

using System.Security.Cryptography;
using System.Text;
using System.IO;


namespace Utilities
{
     public   class Encryptor 
     {
 
         private   static  byte[] _key;
         private   static byte[] _iv;

  
         public static Encryptor CreateEncryptorA()
         {

             return new Encryptor("AdfwR4$128jK");
         }


         public static Encryptor CreateEncryptorB()
         {
    
             return new Encryptor("OKZXR4!128jG");
         }

         private Encryptor(string encryptionKey)
         {

            var salt= Encoding.ASCII.GetBytes(encryptionKey);

            var keyGenerator = new Rfc2898DeriveBytes(encryptionKey, salt);

            _key = keyGenerator.GetBytes(32);

            _iv = keyGenerator.GetBytes(16);

        }

 

        public   string  Encrypt(string text)
        {
            var rijndaelCipher = new RijndaelManaged { Key = _key, IV = _iv };

            byte[] plainText = Encoding.Unicode.GetBytes(text);

            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor())
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

       public  string  Decrypt(string encrypted)
        {
            var rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(encrypted);

            using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(_key, _iv))
            {
                using (var memoryStream = new MemoryStream(encryptedData))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }
        


        }

      

    
       
          
       
    }
}
