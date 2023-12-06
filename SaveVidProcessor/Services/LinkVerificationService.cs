using System.Text.RegularExpressions;

namespace SaveVidProcessor.Services
{
    public static class LinkVerificationService
    {
        private static bool IsInstagramUrl(string url)
        {
            // Define a regular expression pattern for Instagram URLs
            string pattern = @"^(https?://)?(www\.)?instagram\.com/";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Use the IsMatch method to check if the URL matches the pattern
            return regex.IsMatch(url);
        }

        private static bool IsTikTokUrl(string url)
        {
            // Define a regular expression pattern for TikTok URLs
            string pattern = @"^(https?://)?(www\.)?tiktok\.com/@[\w\d-]+/video/\d+";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Use the IsMatch method to check if the URL matches the pattern
            return regex.IsMatch(url);
        }

        public static bool IsValidUrl(string url)
        {
            if (IsInstagramUrl(url) || IsTikTokUrl(url))
            {
                return true;
            }

            return false;
        }
    }
}
