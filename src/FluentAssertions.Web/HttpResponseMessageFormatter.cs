using FluentAssertions.Formatting;
using FluentAssertions.Web.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    public class HttpResponseMessageFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is HttpResponseMessage;

        /// <inheritdoc />
        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var response = (HttpResponseMessage)value;

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("The HTTP response was:");

            AppendResponse(messageBuilder, response);
            AppendRequest(messageBuilder, response.RequestMessage);

            return messageBuilder.ToString().TrimEnd();
        }

        private static void AppendResponse(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            AppendProtocolAndStatusCode(messageBuilder, response);
            AppendHeaders(messageBuilder, response.GetHeaders());
            AppendContentLength(messageBuilder, response);
            AppendContent(response?.Content, messageBuilder);
        }

        private static void AppendRequest(StringBuilder messageBuilder, HttpRequestMessage request)
        {
            messageBuilder.AppendLine();
            if (request == null)
            {
                messageBuilder.AppendLine("The originated HTTP request was <null>.");
                return;
            }
            messageBuilder.AppendLine("The originated HTTP request was:");
            messageBuilder.AppendLine();

            messageBuilder.AppendLine($"{request.Method.ToString().ToUpper()} {request.RequestUri} HTTP {request.Version}");

            AppendHeaders(messageBuilder, request.GetHeaders());
            AppendContentLength(messageBuilder, request);
            AppendContent(request.Content, messageBuilder);
        }

        private static void AppendContent(HttpContent httpContent, StringBuilder messageBuilder)
        {
            if (httpContent == null)
            {
                return;
            }

            Func<Task<string>> contentResolver = async () =>
            {
                var stream = await httpContent.ReadAsStreamAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return new StreamReader(stream).ReadToEnd();
            };
            var content = contentResolver.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

            content = content.BeautifyJson();

            if (!string.IsNullOrEmpty(content))
            {
                messageBuilder.AppendLine();
                messageBuilder.AppendLine(content.Trim());
            }
        }

        private static void AppendProtocolAndStatusCode(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine($@"HTTP/{response.Version} {(int)response.StatusCode} {response.StatusCode}");
        }

        private static void AppendContentLength(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            if (!response.GetHeaders().Any(c => string.Equals(c.Key, "Content-Length", StringComparison.OrdinalIgnoreCase)))
            {
                messageBuilder.AppendLine($"Content-Length: {response.Content?.Headers.ContentLength ?? 0}");
            }
        }

        private static void AppendContentLength(StringBuilder messageBuilder, HttpRequestMessage request)
        {
            if (!request.GetHeaders().Any(c => string.Equals(c.Key, "Content-Length", StringComparison.OrdinalIgnoreCase)))
            {
                messageBuilder.AppendLine($"Content-Length: {request.Content?.Headers.ContentLength ?? 0}");
            }
        }

        private static void AppendHeaders(StringBuilder messageBuilder, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            if (headers.Any())
            {
                var headersPrint = string.Join(Environment.NewLine, headers
                    .Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"));
                messageBuilder.AppendLine(headersPrint);
            }
        }
    }
}