﻿using FluentAssertions.Formatting;
using FluentAssertions.Web.Internal.ContentProcessors;
using System.Text;

namespace FluentAssertions.Web;

internal class HttpResponseMessageFormatter : IValueFormatter
{
    public bool CanHandle(object value) => value is HttpResponseMessage;

    /// <inheritdoc />
    public void Format(object value,
        FormattedObjectGraph formattedGraph,
        FormattingContext context,
        FormatChild formatChild)
    {
        var response = (HttpResponseMessage)value;

        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine();
        messageBuilder.AppendLine();
        messageBuilder.AppendLine("The HTTP response was:");

        Func<Task> contentResolver = async () => await AppendHttpResponseMessage(messageBuilder, response);
        contentResolver.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

        formattedGraph.AddFragment(messageBuilder.ToString());
    }

    private static async Task AppendHttpResponseMessage(StringBuilder messageBuilder, HttpResponseMessage response)
    {
        try
        {
            await AppendResponse(messageBuilder, response);
        }
        catch (Exception e)
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine($"An exception occurred while trying to output the some of the response details: {e}");
        }
        try
        {
            await AppendRequest(messageBuilder, response);
        }
        catch (Exception e)
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine($"An exception occurred while trying to output some of the request details: {e}");
        }
    }

    private static async Task AppendResponse(StringBuilder messageBuilder, HttpResponseMessage response)
    {
        AppendProtocolAndStatusCode(messageBuilder, response);
        Appender.AppendHeaders(messageBuilder, response.GetHeaders());
        AppendContentLength(messageBuilder, response.Content);
        await AppendResponseContent(messageBuilder, response);
    }

    private static async Task AppendRequest(StringBuilder messageBuilder, HttpResponseMessage response)
    {
        messageBuilder.AppendLine();
        var request = response.RequestMessage;
        if (request == null)
        {
            messageBuilder.AppendLine("The originating HTTP request was <null>.");
            return;
        }
        messageBuilder.AppendLine("The originating HTTP request was:");
        messageBuilder.AppendLine();

        messageBuilder.AppendLine($"{request.Method.ToString().ToUpper()} {request.RequestUri} HTTP {request.Version}");

        Appender.AppendHeaders(messageBuilder, request.GetHeaders());
        AppendContentLength(messageBuilder, request.Content);

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
        await Appender.AppendContent(messageBuilder, content, false);
    }

    private static void AppendProtocolAndStatusCode(StringBuilder messageBuilder, HttpResponseMessage response)
    {
        messageBuilder.AppendLine();
        messageBuilder.AppendLine($@"HTTP/{response.Version} {(int)response.StatusCode} {response.StatusCode}");
    }

    private static void AppendContentLength(StringBuilder messageBuilder, HttpContent? content)
    {
        if (content != null && content.Headers.TryGetValues("Content-Length", out var values))
        {
            foreach (var value in values)
            {
                messageBuilder.AppendLine($"Content-Length: {value}");
            }
        }
    }
}