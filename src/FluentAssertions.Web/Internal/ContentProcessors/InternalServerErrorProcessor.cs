using System.Text;

namespace FluentAssertions.Web.Internal.ContentProcessors;

internal class InternalServerErrorProcessor : ProcessorBase
{
    private readonly HttpResponseMessage? _httpResponseMessage;
    private readonly HttpContent? _httpContent;

    public InternalServerErrorProcessor(HttpResponseMessage? httpResponseMessage, HttpContent? httpContent)
    {
        _httpResponseMessage = httpResponseMessage;
        _httpContent = httpContent;
    }

    protected override bool CanHandle() => _httpContent != null
    && _httpResponseMessage != null 
    && _httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError 
    && !_httpContent.IsDisposed();

    protected override async Task Handle(StringBuilder contentBuilder)
    {
        var content = await _httpContent!.ReadAsStringAsync();

        AspNetCore30(contentBuilder, content);

        AspNetCore22(contentBuilder, content);
    }

    private static void AspNetCore30(StringBuilder contentBuilder, string? content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        const string headersText = "HEADERS";
        var headersIndex = content!.IndexOf(headersText, StringComparison.Ordinal);
        if (headersIndex >= 0)
        {
            var exceptionDetails = content.Substring(0, headersIndex).Trim();
            contentBuilder.Append(exceptionDetails);
        }
    }

    private static void AspNetCore22(StringBuilder contentBuilder, string? content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        const string startTag = @"<pre class=""rawExceptionStackTrace"">";
        if (content!.Contains(startTag))
        {
            var startTagIndex = content.LastIndexOf(startTag, StringComparison.Ordinal);
            var endTagIndex = content.IndexOf("</pre>", startTagIndex, StringComparison.Ordinal);
            if (endTagIndex < 0)
            {
                // there is no end tag
                return;
            }

            var exceptionDetails = content.Substring(startTagIndex + startTag.Length, endTagIndex - startTagIndex - startTag.Length);

            exceptionDetails = WebUtility.HtmlDecode(exceptionDetails);

            contentBuilder.Append(exceptionDetails);
        }
    }
}
