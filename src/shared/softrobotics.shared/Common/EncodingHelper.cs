using System.Security.Cryptography;
using System.Text;

namespace softrobotics.shared.Common
{
    public static class EncodingHelper
    {
        public static string EncodeSHA256(this string str) =>
            BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
    }
}