using System.Text;

namespace FluentAssertions.Web.Internal.ContentProcessors;

internal class BinaryProcessor : ProcessorBase
{
    private readonly HttpContent? _httpContent;

    public BinaryProcessor(HttpContent? httpContent)
    {
        _httpContent = httpContent;
    }

    protected override Task Handle(StringBuilder contentBuilder)
    {
        _httpContent!.TryGetContentLength(out var contentLength);

        contentBuilder.AppendLine(string.Format(ContentFormatterOptions.ContentIsOfABinaryEncodedTypeHavingLength, contentLength));

        return Task.CompletedTask;
    }

    protected override bool CanHandle() => _httpContent != null
                                           && !_httpContent.IsDisposed()
                                           && _httpContent is not (StringContent or FormUrlEncodedContent)
                                           && (IsNonPrintableContent(_httpContent) || _httpContent is ByteArrayContent);
    private static bool IsNonPrintableContent(HttpContent? content)
    {
        // a file name is passed
        if (!string.IsNullOrEmpty(content?.Headers?.ContentDisposition?.FileName))
        {
            return true;
        }

        var contentTypeMediaType = content?.Headers?.ContentType?.MediaType;
        if (contentTypeMediaType == null)
        {
            return false;
        }

        // exclude all know "text" like content
        if (contentTypeMediaType.Contains("xml")
            || contentTypeMediaType.Contains("json")
            || contentTypeMediaType.Contains("html")
            || contentTypeMediaType.Contains("text")
           )
        {
            return false;
        }

        // all audio + video + image
        if (contentTypeMediaType.StartsWith("audio/")
            || contentTypeMediaType.StartsWith("image/")
            || contentTypeMediaType.StartsWith("video/"))
        {
            return true;
        }

        // pdfs
        if (contentTypeMediaType.Contains("pdf"))
        {
            return true;
        }

        // don't know really if it is a binary type, so better to return that it is not, maybe others will continue asking what it is
        return false;
    }
}
