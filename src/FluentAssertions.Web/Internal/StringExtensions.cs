#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

internal static class StringExtensions
{
    public static string ReplaceFirstWithLowercase(this string source) => !string.IsNullOrEmpty(source) ?
        source[0].ToString().ToLower() + source.Substring(1)
        : source;

    public static string TrimDot(this string source) => source.TrimEnd('.');
}