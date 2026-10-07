namespace HttpMessageFormatter.Internal.ContentProcessors;

internal abstract class ProcessorBase : IContentProcessor
{
    protected ProcessorBase(HttpResponseFormatterOptions? options) => Options = options;

    protected HttpResponseFormatterOptions? Options { get; }

    public async Task GetContentInfo(StringBuilder contentBuilder)
    {
        if (!CanHandle())
        {
            return;
        }

        await Handle(contentBuilder);
    }

    protected abstract bool CanHandle();
    protected abstract Task Handle(StringBuilder contentBuilder);

    protected void AppendContentWithinLimits(StringBuilder contentBuilder, string? content)
    {
        if (content == null)
        {
            return;
        }

        var maximumReadableBytes = Options?.MaximumReadableBytes ?? ContentFormatterOptions.MaximumReadableBytes;
        var toAppend = content;
        var contentLength = content.Length;

        if (contentLength >= maximumReadableBytes)
        {
            contentBuilder.AppendLine();
            contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenContentIsTooLarge);
            toAppend = content!.Substring(0, maximumReadableBytes);
        }

        contentBuilder.Append(toAppend);
    }
}
