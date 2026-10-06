namespace HttpMessageFormatter.Internal.ContentProcessors;

internal static class ProcessorsRunner
{
    public static async Task<StringBuilder> RunProcessors(IEnumerable<IContentProcessor> processors)
    {
        var contentBuilder = new StringBuilder();
        foreach (var processor in processors)
        {
            await processor.GetContentInfo(contentBuilder);
        }

        return contentBuilder;
    }

    public static IReadOnlyCollection<IContentProcessor> CommonProcessors(HttpContent content, HttpResponseFormatterOptions? options = null) => new IContentProcessor[]
    {
        new JsonProcessor(content, options),
        new BinaryProcessor(content, options),
        new MultipartProcessor(content, options),
        new FallbackProcessor(content, options)
    };
}
