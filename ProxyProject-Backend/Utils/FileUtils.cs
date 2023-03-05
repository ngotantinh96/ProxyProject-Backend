namespace ProxyProject_Backend.Utils
{
    public static class FileUtils
    {
        public static bool IsFileExtensionAllowed(string fileName, string[] allowedExtensions)
        {
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
