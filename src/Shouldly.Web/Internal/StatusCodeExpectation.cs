using System.Diagnostics;
using System.Net;

namespace Shouldly.Web.Internal;

/// <summary>
/// The expected side of a status-code assertion: a predicate over the numeric
/// status code plus the display text shown in the failure message. The display
/// is returned from <see cref="ToString"/>, so Shouldly renders it as prose
/// rather than quoting it like a string.
/// </summary>
[DebuggerStepThrough]
internal readonly struct StatusCodeExpectation
{
    private readonly Func<int, bool> matches;
    private readonly string display;

    internal StatusCodeExpectation(Func<int, bool> matches, string display)
    {
        this.matches = matches;
        this.display = display;
    }

    internal bool Matches(int statusCode) => matches(statusCode);

    public override string ToString() => display;

    /// <summary>An exact numeric status code, displayed with the text master prints for it.</summary>
    internal static StatusCodeExpectation Exactly(int code, string display)
        => new(statusCode => statusCode == code, display);

    internal static StatusCodeExpectation Informational()
        => new(statusCode => ((HttpStatusCode)statusCode).IsInformational(), "a 1XX informational status code");

    internal static StatusCodeExpectation Successful()
        => new(statusCode => ((HttpStatusCode)statusCode).IsSuccessful(), "a 2XX successful status code");

    /// <summary>301-399: 300 Multiple Choices is deliberately not a redirection, mirroring master.</summary>
    internal static StatusCodeExpectation Redirection()
        => new(statusCode => ((HttpStatusCode)statusCode).IsRedirection(), "a 3XX redirection status code (301\u2013399)");

    internal static StatusCodeExpectation ClientError()
        => new(statusCode => ((HttpStatusCode)statusCode).IsClientError(), "a 4XX client error status code");

    internal static StatusCodeExpectation ServerError()
        => new(statusCode => ((HttpStatusCode)statusCode).IsServerError(), "a 5XX server error status code");
}
