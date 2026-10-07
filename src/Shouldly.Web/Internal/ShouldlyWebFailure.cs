using System.Diagnostics;

namespace Shouldly.Web.Internal;

/// <summary>
/// Builds this library's failure messages: Shouldly's own message, then the
/// HTTP response dump from <c>HttpMessageFormatter</c> appended to it.
/// </summary>
[DebuggerStepThrough]
internal static class ShouldlyWebFailure
{
    /// <summary>HTTP-specific semantics: build Shouldly's own message, then append the dump.</summary>
    internal static ShouldAssertException Create(HttpResponseMessage response, ShouldlyMessage message)
        => new(message.ToString() + Dump(response));

    /// <summary>A native Shouldly assertion already says it best: run it, and on failure re-throw with the dump appended.</summary>
    internal static void Rethrow(HttpResponseMessage response, Action nativeAssertion)
    {
        try
        {
            nativeAssertion();
        }
        catch (ShouldAssertException ex)
        {
            throw new ShouldAssertException(ex.Message + Dump(response), ex);
        }
    }

    private static string Dump(HttpResponseMessage response)
        => response.Format(AssertionsWebConfig.ResponseFormatterOptions);
}
