using FluentAssertions.Formatting;
using FluentAssertions.Web.Internal;
using FluentAssertions.Web.Internal.ContentProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    internal class HttpResponseMessageFormatter : IValueFormatter
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

            Func<Task> contentResolver = async () => await AppendHttpResponseMessage(messageBuilder, response);
            contentResolver.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

            return messageBuilder.ToString();
        }

        private static async Task AppendHttpResponseMessage(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            await AppendResponse(messageBuilder, response);
            await AppendRequest(messageBuilder, response);
        }

        private static async Task AppendResponse(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            AppendProtocolAndStatusCode(messageBuilder, response);
            Appender.AppendHeaders(messageBuilder, response.GetHeaders());
            AppendContentLength(messageBuilder, response);
            await AppendResponseContent(messageBuilder, response);
        }

        private static async Task AppendRequest(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            var request = response.RequestMessage;
            messageBuilder.AppendLine();
            if (request == null)
            {
                messageBuilder.AppendLine("The originated HTTP request was <null>.");
                return;
            }
            messageBuilder.AppendLine("The originated HTTP request was:");
            messageBuilder.AppendLine();

            messageBuilder.AppendLine($"{request.Method.ToString().ToUpper()} {request.RequestUri} HTTP {request.Version}");

            Appender.AppendHeaders(messageBuilder, request.GetHeaders());
            AppendContentLength(messageBuilder, request);

            messageBuilder.AppendLine();

            await AppendRequestContent(messageBuilder, request.Content);
        }

        private static async Task AppendResponseContent(StringBuilder messageBuilder, HttpResponseMessage response)
        {
            var content = response.Content;
            if (content == null)
            {
                return;
            }

            var processors = new List<IContentProcessor>();
            processors.Add(new InternalServerErrorProcessor(response, content));
            processors.AddRange(ProcessorsRunner.CommonProcessors(content));

            var contentBuilder = await ProcessorsRunner.RunProcessors(processors);
            messageBuilder.AppendLine();
            messageBuilder.Append(contentBuilder);
        }

        private static async Task AppendRequestContent(StringBuilder messageBuilder, HttpContent content)
        {
            await Appender.AppendContent(messageBuilder, content);
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
            if (!request.GetHeaders()
                .Any(c => string.Equals(c.Key, "Content-Length", StringComparison.OrdinalIgnoreCase))
            )
            {
                request.Content.TryGetContentLength(out long contentLength);
                messageBuilder.AppendLine($"Content-Length: {contentLength}");
            }
        }
    }
}