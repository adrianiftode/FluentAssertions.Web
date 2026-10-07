using System.Text.RegularExpressions;

namespace Assertions.Web.Internal;

internal static class StringExtensions
{
    public static string ReplaceFirstWithLowercase(this string source) => !string.IsNullOrEmpty(source) ?
        source[0].ToString().ToLower() + source.Substring(1)
        : source;

    public static string TrimDot(this string source) => source.TrimEnd('.');

    /// <summary>
    /// Matches <paramref name="actual"/> against a shell-like pattern where
    /// <c>*</c> matches any sequence of characters and <c>?</c> matches exactly one.
    /// The match is anchored, case-sensitive and culture-invariant.
    /// </summary>
    public static bool WildcardMatch(this string? actual, string pattern)
    {
        if (actual is null)
        {
            return false;
        }

        var regex = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        return Regex.IsMatch(actual, regex, RegexOptions.Singleline | RegexOptions.CultureInvariant);
    }
}