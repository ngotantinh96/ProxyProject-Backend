using System.Security.Cryptography;
using System.Text;

namespace ProxyProject_Backend.Utils
{
    public static class StringUtils
    {
        private const int _numberOfSecureBytesToGenerate = 32;
        private const int _lengthOfKey = 32;

        public static string GenerateSecureKey()
        {
            var bytes = RandomNumberGenerator.GetBytes(_numberOfSecureBytesToGenerate);

            string base64String = Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_");

            return base64String[.._lengthOfKey];
        }

        // Generates a key of the specified size
        private static byte[] GenerateKey(int keySize)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public static string Encrypt(string plaintext, string key)
        {
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] ciphertextBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                byte[] ivAndCiphertextBytes = new byte[aes.IV.Length + ciphertextBytes.Length];
                Buffer.BlockCopy(aes.IV, 0, ivAndCiphertextBytes, 0, aes.IV.Length);
                Buffer.BlockCopy(ciphertextBytes, 0, ivAndCiphertextBytes, aes.IV.Length, ciphertextBytes.Length);

                return Convert.ToBase64String(ivAndCiphertextBytes);
            }
        }

        public static string Decrypt(string ciphertext, string key)
        {
            byte[] ivAndCiphertextBytes = Convert.FromBase64String(ciphertext);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] ivBytes = new byte[aes.IV.Length];
                Buffer.BlockCopy(ivAndCiphertextBytes, 0, ivBytes, 0, aes.IV.Length);

                byte[] ciphertextBytes = new byte[ivAndCiphertextBytes.Length - aes.IV.Length];
                Buffer.BlockCopy(ivAndCiphertextBytes, aes.IV.Length, ciphertextBytes, 0, ciphertextBytes.Length);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, ivBytes);
                byte[] plaintextBytes = decryptor.TransformFinalBlock(ciphertextBytes, 0, ciphertextBytes.Length);

                return Encoding.UTF8.GetString(plaintextBytes);
            }
        }
    }
}
