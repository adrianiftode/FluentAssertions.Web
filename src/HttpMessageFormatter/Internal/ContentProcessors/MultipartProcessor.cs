namespace HttpMessageFormatter.Internal.ContentProcessors;

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
        else if (_httpContent is MultipartContent generalMultipartContent)
        {
            multipartContent = generalMultipartContent;
        }
        else if (_httpContent is StreamContent streamContent)
        {
            var stream = await streamContent.ReadAsStreamAsync();
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            if (_httpContent.Headers.ContentType?.MediaType?.StartsWith("multipart/", System.StringComparison.OrdinalIgnoreCase) == true)
            {
                multipartContent = await _httpContent.ReadAsMultipartExtensionAsync();
            }
        }

        if (multipartContent == null)
        {
            return;
        }

        var boundary = GetBoundary();

        try
        {

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var content in multipartContent)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(boundary);

                Appender.AppendHeaders(contentBuilder, content.Headers);

                await Appender.AppendContent(contentBuilder, content, true);
            }
        }
        finally
        {
            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var content in multipartContent)
            {
                content.Dispose();
            }
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

    private string? GetBoundary()
    {
        var contentType = _httpContent?.Headers?.ContentType;

        var boundaryParameter = contentType?.Parameters?.FirstOrDefault(p => p.Name.Equals("boundary", StringComparison.OrdinalIgnoreCase));
        return boundaryParameter != null ? $"--{boundaryParameter.Value}" : null;
    }
}