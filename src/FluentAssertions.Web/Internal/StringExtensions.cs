using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
}