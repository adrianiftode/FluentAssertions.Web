namespace Assertions.Web.Internal;

internal static class ContentFormatterOptions
{
    public const int MaximumReadableBytes = 10 * 128 * 1024; // 1KB holds like 500 words
}