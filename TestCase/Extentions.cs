using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TestCase.Models;

namespace TestCase
{
    public static class Extentions
    {
        public static string Base64UrlEncode(this string someString)
        {
            var bytes = Encoding.UTF8.GetBytes(someString);

            return bytes.Base64UrlEncode();
        }

        public static string Base64UrlEncode(this byte[] bytes)
        {
            var base64 = Convert.ToBase64String(bytes);
            var base64Url = base64.Replace("=", "").Replace('+', '-').Replace('/', '_');
            return base64Url;
        }

        public static string GenerateSignature(this Signature signature)
        {
            ASCIIEncoding encoding = new();
            var cipherText = $"{signature.Base64Header}.{signature.Base64Payload}";

            HMACSHA256 hmacSha256 = new HMACSHA256(encoding.GetBytes(signature.SecretKey));

            var result = encoding.GetBytes(cipherText);

            var hashResult = hmacSha256.ComputeHash(result);

            return hashResult.Base64UrlEncode();
        }
    }

}
