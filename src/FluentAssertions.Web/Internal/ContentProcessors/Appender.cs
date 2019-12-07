using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal static class Appender
    {
        public static void AppendHeaders(StringBuilder messageBuilder, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            foreach (var header in headers)
            {
                var headersPrint = $"{header.Key}: {string.Join(", ", header.Value)}";
                messageBuilder.AppendLine(headersPrint);
            }
        }
        public static async Task AppendContent(StringBuilder contentBuilder, HttpContent content)
        {
            var partContentBuilder = await ProcessorsRunner.RunProcessors(ProcessorsRunner.CommonProcessors(content));

            if (partContentBuilder.Length > 0)
            {
                contentBuilder.AppendLine();
                contentBuilder.Append(partContentBuilder);
            }
        }
    }
}