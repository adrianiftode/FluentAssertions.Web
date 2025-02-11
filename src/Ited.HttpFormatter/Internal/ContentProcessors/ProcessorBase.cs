using System.Text;

namespace Ited.HttpFormatter.Internal.ContentProcessors;

internal abstract class ProcessorBase : IContentProcessor
{
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

        var toAppend = content;
        var contentLength = content.Length;

        if (contentLength >= ContentFormatterOptions.MaximumReadableBytes)
        {
            contentBuilder.AppendLine();
            contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenContentIsTooLarge);
            toAppend = content!.Substring(0, ContentFormatterOptions.MaximumReadableBytes);
        }

        contentBuilder.Append(toAppend);
    }
}