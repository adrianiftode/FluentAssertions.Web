using System.Text;

namespace Ited.HttpFormatter.Internal.ContentProcessors;

internal static class Appender
{
    public static void AppendHeaders(StringBuilder messageBuilder, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        foreach (var header in headers)
        {
            if (header.Key != "Content-Length")
            {
                foreach (var headerValue in header.Value)
                {
                    messageBuilder.AppendLine($"{header.Key}: {headerValue}");
                }
            }
        }
    }

    public static async Task AppendContent(StringBuilder contentBuilder, HttpContent content, bool appendLineBeforeContent)
    {
        var partContentBuilder = await ProcessorsRunner.RunProcessors(ProcessorsRunner.CommonProcessors(content));

        if (partContentBuilder.Length > 0)
        {
            if (appendLineBeforeContent)
            {
                contentBuilder.AppendLine();
            }

            contentBuilder.Append(partContentBuilder);
        }
    }
}