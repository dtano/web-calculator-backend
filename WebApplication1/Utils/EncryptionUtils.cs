using System.Text;
using System.Security.Cryptography;

namespace WebApplication1.Utils
{
    public class EncryptionUtils
    {
        public static string SecretKey = "secret-key-for-app";
        public static string Encrypt(string plainText)
        {
            try
            {
                // Convert the plaintext string to a byte array
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

                // Derive a new password using the PBKDF2 algorithm and a random salt
                Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(SecretKey, 20);

                // Use the password to encrypt the plaintext
                Aes encryptor = Aes.Create();
                encryptor.Key = passwordBytes.GetBytes(32);
                encryptor.IV = passwordBytes.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plaintextBytes, 0, plaintextBytes.Length);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in encryption" + ex.Message);
            }
        }

        public string Decode(string encryptedData)
        {
            // Convert the encrypted string to a byte array
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // Derive the password using the PBKDF2 algorithm
            Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(SecretKey, 20);

            // Use the password to decrypt the encrypted string
            Aes encryptor = Aes.Create();
            encryptor.Key = passwordBytes.GetBytes(32);
            encryptor.IV = passwordBytes.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
