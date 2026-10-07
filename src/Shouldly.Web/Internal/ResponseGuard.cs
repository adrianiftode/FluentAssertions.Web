using System.Diagnostics;

namespace Shouldly.Web.Internal;

/// <summary>
/// Guards the response under test before any assertion runs on it. The null
/// failure is a plain Shouldly failure and carries no HTTP dump.
/// </summary>
[DebuggerStepThrough]
internal static class ResponseGuard
{
    internal static HttpResponseMessage EnsureNotNull(HttpResponseMessage? actual, string? customMessage, string? actualExpression)
    {
        actual.ShouldNotBeNull(customMessage, actualExpression);

        // Shouldly's annotations do not narrow nullability here, and CS8603 is an error in this repo.
        return actual!;
    }
}
