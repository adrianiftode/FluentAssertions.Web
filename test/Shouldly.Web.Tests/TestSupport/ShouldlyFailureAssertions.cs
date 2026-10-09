namespace Shouldly.Web.Tests.TestSupport;

/// <summary>
/// Assertion helpers used by the <c>SH</c> branches of the shared specs.
/// </summary>
public static class ShouldlyFailureAssertions
{
    /// <summary>
    /// Fails unless <paramref name="act"/> throws a <see cref="ShouldAssertException"/> whose message
    /// starts with <paramref name="expression"/> on its own line and contains every fragment, in order,
    /// compared ordinally after normalising line endings.
    /// </summary>
    /// <returns>The exception message, with line endings normalised to <c>\n</c>.</returns>
    public static string ShouldFailWith(this Action act, string expression, params string[] fragmentsInOrder)
    {
        var message = Normalise(Capture(act));

        var newlineIndex = message.IndexOf('\n');
        var firstLine = newlineIndex < 0 ? message : message.Substring(0, newlineIndex);
        if (!string.Equals(firstLine, expression, StringComparison.Ordinal))
        {
            throw new ShouldAssertException(
                $"Expected the failure message to start with the line:{Lines(expression)}but it started with:{Lines(firstLine)}The full message was:{Lines(message)}");
        }

        return CheckFragments(message, fragmentsInOrder);
    }

    /// <summary>
    /// Same as <see cref="ShouldFailWith"/> but without the first-line check. Only for the equivalency
    /// layout, which starts with "Comparing object equivalence".
    /// </summary>
    /// <returns>The exception message, with line endings normalised to <c>\n</c>.</returns>
    public static string ShouldFailContaining(this Action act, params string[] fragmentsInOrder)
        => CheckFragments(Normalise(Capture(act)), fragmentsInOrder);

    private static string Capture(Action act)
    {
        try
        {
            act();
        }
        catch (ShouldAssertException ex)
        {
            return ex.Message;
        }
        catch (Exception ex)
        {
            throw new ShouldAssertException(
                $"Expected a {nameof(ShouldAssertException)} but the action threw {ex.GetType().FullName}:{Lines(ex.Message)}", ex);
        }

        throw new ShouldAssertException("Expected the action to fail but it succeeded.");
    }

    private static string CheckFragments(string message, string[] fragmentsInOrder)
    {
        var position = 0;
        foreach (var fragment in fragmentsInOrder)
        {
            var normalisedFragment = Normalise(fragment);
            var index = message.IndexOf(normalisedFragment, position, StringComparison.Ordinal);
            if (index < 0)
            {
                throw new ShouldAssertException(
                    $"Expected the failure message to contain the fragment:{Lines(normalisedFragment)}after position {position}, but it did not. The full message was:{Lines(message)}");
            }

            position = index + normalisedFragment.Length;
        }

        return message;
    }

    private static string Normalise(string text) => text.Replace("\r\n", "\n").Replace("\r", "\n");

    private static string Lines(string text) => $"{text}\n";
}
