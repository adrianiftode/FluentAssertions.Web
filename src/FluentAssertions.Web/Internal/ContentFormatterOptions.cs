#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

internal static class ContentFormatterOptions
{
    public const int MaximumReadableBytes = 10 * 128 * 1024; // 1KB holds like 500 words
}