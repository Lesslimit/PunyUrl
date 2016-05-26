using System;
using System.Text;

namespace PunyUrl.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string ToBase64(this string value, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            return Convert.ToBase64String(encoding.GetBytes(value));
        }

        public static string FromBase64(this string value, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            return encoding.GetString(Convert.FromBase64String(value));
        }
    }
}