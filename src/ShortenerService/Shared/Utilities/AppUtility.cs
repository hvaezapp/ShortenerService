using System.Security.Cryptography;
using System.Text;

namespace ShortenerService.Shared.Utilities
{
    public static class AppUtility
    {
        public static string GenerateCode(string longUrl)
        {
            var hashBytes =  MD5.HashData(Encoding.UTF8.GetBytes(longUrl));
            var hashCode = Convert.ToHexStringLower(hashBytes);
            return hashCode[5..];
        }
    }
}
