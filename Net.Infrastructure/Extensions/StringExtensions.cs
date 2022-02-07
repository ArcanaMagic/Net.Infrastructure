using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Net.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            length = Math.Abs(length);

            return value.Length <= length ? value : value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        public static string FindTextBetween(this string text, string left, string right)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty; // or throw exception?

            var beginIndex = text.IndexOf(left, StringComparison.Ordinal); 
            if (beginIndex == -1)
                return string.Empty; // or throw exception?

            beginIndex += left.Length;

            int endIndex = text.IndexOf(right, beginIndex, StringComparison.Ordinal); 
            if (endIndex == -1)
                return string.Empty; // or throw exception?

            return text[beginIndex..endIndex].Trim();
        }

        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((character, index) =>
                    index > 0 && char.IsUpper(character)
                        ? "_" + character
                        : character.ToString()))
                .ToLower();
        }

        public static Uri ToUri(this string text)
        {
            return new Uri(text);
        }

        public static string ToSha256Hash(this string text)
        {
            // Create a SHA256   
            using var sha256Hash = SHA256.Create();

            // ComputeHash - returns byte array  
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Convert byte array to a string   
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public static string ToHmac(this string text, string key)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            var keyBytes = HexDecode(key);

            byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))
            {
                hashBytes = hash.ComputeHash(textBytes);
            }

            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}