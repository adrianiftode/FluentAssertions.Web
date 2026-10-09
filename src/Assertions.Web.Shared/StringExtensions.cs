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
    /// <remarks>
    /// A linear two-pointer glob matcher rather than a regex: <c>?</c> consumes one UTF-16 code unit and
    /// <c>*</c> crosses newlines, so it is behaviourally identical to the previous anchored
    /// <c>RegexOptions.Singleline</c> match, but it cannot back-track catastrophically on patterns such as
    /// <c>*a*b*c*</c>. <c>WildcardMatchTests</c> cross-checks the pairs against FluentAssertions'
    /// <c>Should().Match</c>.
    /// </remarks>
    public static bool WildcardMatch(this string? actual, string pattern)
    {
        if (actual is null)
        {
            return false;
        }

        var actualIndex = 0;
        var patternIndex = 0;
        var lastStarIndex = -1;
        var indexAfterLastStar = 0;

        while (actualIndex < actual.Length)
        {
            if (patternIndex < pattern.Length &&
                (pattern[patternIndex] == '?' || pattern[patternIndex] == actual[actualIndex]))
            {
                actualIndex++;
                patternIndex++;
            }
            else if (patternIndex < pattern.Length && pattern[patternIndex] == '*')
            {
                lastStarIndex = patternIndex;
                indexAfterLastStar = actualIndex;
                patternIndex++;
            }
            else if (lastStarIndex != -1)
            {
                // Back-track to the last '*', letting it consume one more character of the actual value.
                patternIndex = lastStarIndex + 1;
                indexAfterLastStar++;
                actualIndex = indexAfterLastStar;
            }
            else
            {
                return false;
            }
        }

        // Any remaining pattern must be only '*' (each matching the empty remainder).
        while (patternIndex < pattern.Length && pattern[patternIndex] == '*')
        {
            patternIndex++;
        }

        return patternIndex == pattern.Length;
    }
}
