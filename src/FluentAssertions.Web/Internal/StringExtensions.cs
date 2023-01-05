namespace FluentAssertions.Web.Internal;

internal static class StringExtensions
{
    public static string ReplaceFirstWithLowercase(this string source) => !string.IsNullOrEmpty(source) ?
        source[0].ToString().ToLower() + source.Substring(1)
        : source;

    public static string TrimDot(this string source) => source.TrimEnd('.');

    public static bool EqualsCaseInsensitive(this string? source, string? other)
        => string.Equals(source, other, StringComparison.OrdinalIgnoreCase);
}