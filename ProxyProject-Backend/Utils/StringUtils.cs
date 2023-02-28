using System.Security.Cryptography;

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
    }
}
