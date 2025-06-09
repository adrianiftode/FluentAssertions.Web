#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

internal static class ContentFormatterOptions
{
    public const int MaximumReadableBytes = 10 * 128 * 1024; // 1KB holds like 500 words
    public const string WarningMessageWhenDisposed = "***** Content is disposed so it cannot be read. *****";
    public const string WarningMessageWhenContentIsTooLarge = "***** Content is too large to display and only a part is printed. *****";
    public const string ContentIsOfABinaryEncodedTypeHavingLength = "***** Content is of a binary encoded like type having the length {0}. *****";
}