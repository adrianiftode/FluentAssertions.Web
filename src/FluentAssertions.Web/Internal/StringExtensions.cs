using Newtonsoft.Json.Linq;

namespace FluentAssertions.Web.Internal
{
    internal static class StringExtensions
    {
        public static string EscapePlaceholders(this string value) =>
            value.Replace("{", "{{").Replace("}", "}}");

        public static string BeautifyJson(this string content)
        {
            string result;
            try
            {
                result = JToken.Parse(content).ToString().Replace("  ", "    ");
            }
            catch
            {
                result = content;
            }

            return result;
        }
    }
}