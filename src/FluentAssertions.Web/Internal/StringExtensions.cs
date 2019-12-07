using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace FluentAssertions.Web.Internal
{
    internal static class StringExtensions
    {
        public static string BeautifyJson(this string content)
        {
            string result;
            try
            {
                result = JToken.Parse(content).ToString().Replace("  ", "    ");
            }
            catch (JsonReaderException)
            {
                result = content;
            }

            return result;
        }

        public static string ReplaceFirstWithLowercase(this string source) => !string.IsNullOrEmpty(source) ?
            source[0].ToString().ToLower() + source.Substring(1)
            : source;

        public static bool EqualsCaseInsensitive(this string? source, string? other)
            => string.Equals(source, other, StringComparison.OrdinalIgnoreCase);
    }
}