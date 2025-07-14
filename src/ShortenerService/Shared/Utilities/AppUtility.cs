using System.Security.Cryptography;
using System.Text;

namespace ShortenerService.Shared.Utilities
{
    public static class AppUtility
    {
        public static string GenerateCode(string longUrl)
        {
            using MD5 md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
            var hashCode = Convert.ToHexStringLower(hashBytes);

            return hashCode[10..];
        }
    }
}
