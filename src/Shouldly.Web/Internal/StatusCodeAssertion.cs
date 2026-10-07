using System.Diagnostics;

namespace Shouldly.Web.Internal;

/// <summary>
/// Runs a <see cref="StatusCodeExpectation"/> against a response and, when it
/// does not hold, throws this library's failure: Shouldly's expected/actual
/// message with the HTTP dump appended.
/// </summary>
[DebuggerStepThrough]
internal static class StatusCodeAssertion
{
    internal static void Assert(HttpResponseMessage? actual, StatusCodeExpectation expected, string? customMessage, string? actualExpression)
    {
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        if (!expected.Matches((int)response.StatusCode))
        {
            throw ShouldlyWebFailure.Create(response, new ExpectedActualShouldlyMessage(
                expected,
                response.StatusCode,
                customMessage,
                shouldlyMethod: "ShouldBe",
                actualExpression: $"{actualExpression}.StatusCode"));
        }
    }
}
