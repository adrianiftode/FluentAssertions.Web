using System.Diagnostics;
using System.Threading;

namespace Shouldly.Web.Internal;

/// <summary>
/// Builds this library's failure messages: Shouldly's own message, then the
/// HTTP response dump from <c>HttpMessageFormatter</c> appended to it.
/// </summary>
[DebuggerStepThrough]
internal static class ShouldlyWebFailure
{
    /// <summary>
    /// While an outer assertion is running its inner assertions, a nested failure must not append its own
    /// HTTP dump: the outer one appends the single dump once the whole assertion has failed. Without this,
    /// a <c>ShouldSatisfy</c> holding several Shouldly.Web conditions would format the same response once
    /// per failing condition. <see cref="AsyncLocal{T}"/> is used because the async assertions run their
    /// inner assertions through <c>ExecuteInDefaultSynchronizationContext</c>.
    /// </summary>
    private static readonly AsyncLocal<int> SuppressDepth = new();

    /// <summary>HTTP-specific semantics: build Shouldly's own message, then append the dump.</summary>
    internal static ShouldAssertException Create(HttpResponseMessage response, ShouldlyMessage message)
        => new(message.ToString() + Dump(response));

    /// <summary>
    /// A native Shouldly assertion already says it best: run it while nested dumps are suppressed, and on
    /// failure re-throw with the dump appended once. Suppressing during the call also avoids the N+1
    /// formatter runs caused by Shouldly.Web assertions nested inside one another.
    /// </summary>
    internal static void Rethrow(HttpResponseMessage response, Action nativeAssertion)
    {
        try
        {
            using (SuppressDump())
            {
                nativeAssertion();
            }
        }
        catch (ShouldAssertException ex)
        {
            // Deliberately no inner exception: test runners print the inner exception's message after the
            // outer one, which would show this failure (and its error list) a second time.
            throw new ShouldAssertException(ex.Message + Dump(response));
        }
    }

    /// <summary>A model could not be deserialized from the response: report the model type and the underlying error.</summary>
    internal static ShouldAssertException Deserialization(HttpResponseMessage response, Type modelType,
        string? errorMessage, string? customMessage, string shouldlyMethod, string actualExpression)
        => Create(response, new ExpectedActualShouldlyMessage(modelType, errorMessage, customMessage, shouldlyMethod,
            actualExpression));

    /// <summary>
    /// The warning the formatter prints before a truncated content. Mirrored here because an assertion
    /// whose own message prints the body must respect the same
    /// <see cref="HttpResponseFormatterOptions.MaximumReadableBytes"/> limit as the HTTP dump.
    /// </summary>
    internal const string ContentIsTooLargeWarning =
        "***** Content is too large to display and only a part is printed. *****";

    /// <summary>
    /// Cuts the content to <see cref="HttpResponseFormatterOptions.MaximumReadableBytes"/> and appends the
    /// formatter's "too large" warning, so a failure message that shows the body stays bounded. Matching
    /// still runs against the full content; only the value printed in the message is shortened.
    /// </summary>
    internal static string? TruncateContentForMessage(string? content)
    {
        if (content is null)
        {
            return content;
        }

        var maximumReadableBytes = AssertionsWebConfig.ResponseFormatterOptions.MaximumReadableBytes;
        if (content.Length < maximumReadableBytes)
        {
            return content;
        }

        return content.Substring(0, maximumReadableBytes) + Environment.NewLine + ContentIsTooLargeWarning;
    }

    /// <summary>
    /// Suppresses the HTTP dump while the returned scope is alive. Used by the callers that run nested
    /// Shouldly.Web assertions, so only the outermost failure formats the response.
    /// </summary>
    internal static IDisposable SuppressDump()
    {
        SuppressDepth.Value++;
        return new DumpSuppression();
    }

    private static string Dump(HttpResponseMessage response)
        => SuppressDepth.Value > 0 ? string.Empty : response.Format(AssertionsWebConfig.ResponseFormatterOptions);

    private sealed class DumpSuppression : IDisposable
    {
        public void Dispose() => SuppressDepth.Value--;
    }
}
