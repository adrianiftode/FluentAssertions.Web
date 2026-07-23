namespace HttpMessageFormatter.Internal;

internal static class StringExtensions
{
    public static string ReplaceFirstWithLowercase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }

    public static bool EqualsCaseInsensitive(this string? input, string? compare)
    {
        return string.Equals(input, compare, StringComparison.OrdinalIgnoreCase);
    }
}
