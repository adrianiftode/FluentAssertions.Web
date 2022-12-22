using System.Text;

namespace FluentAssertions.Web.Internal.ContentProcessors;

internal class MultipartProcessor : ProcessorBase
{
    private readonly HttpContent? _httpContent;
    public MultipartProcessor(HttpContent? httpContent)
    {
        _httpContent = httpContent;
    }

    protected override async Task Handle(StringBuilder contentBuilder)
    {
        IEnumerable<HttpContent>? multipartContent = null;
        if (_httpContent is MultipartFormDataContent dataContent)
        {
            multipartContent = dataContent;
        }
        else
        {
            if (_httpContent is StreamContent streamContent)
            {
                var stream = await streamContent.ReadAsStreamAsync();
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                multipartContent = (await _httpContent.ReadAsMultipartAsync(new MultipartMemoryStreamProvider())).Contents;
            }
        }

        if (multipartContent == null)
        {
            return;
        }

        var boundary = GetBoundary();

        foreach (var content in multipartContent)
        {
            contentBuilder.AppendLine();
            contentBuilder.AppendLine(boundary);

            Appender.AppendHeaders(contentBuilder, content.Headers);

            await Appender.AppendContent(contentBuilder, content, true);
        }

        contentBuilder.AppendLine();
        contentBuilder.AppendLine($"{boundary}--");
    }

    protected override bool CanHandle()
    {
        if (_httpContent is MultipartContent)
        {
            return true;
        }

        var boundary = GetBoundary();

        return !string.IsNullOrEmpty(boundary);
    }

    private string? GetBoundary() => _httpContent?.Headers?.ContentType?.Parameters.FirstOrDefault(c => c.Name == "boundary")?.Value?.Trim('\"');
}