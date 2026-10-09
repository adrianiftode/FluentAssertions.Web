using System.Net;

namespace Assertions.Web.Internal;

/// <summary>
/// Provides predicates over <see cref="HttpStatusCode"/> values for the five
/// status-code families. The ranges are exactly the ones the assertions have
/// always used: informational is below 200, successful is 200-299, and
/// redirection starts at 301 (300 itself is not treated as a redirection).
/// </summary>
internal static class HttpStatusCodeExtensions
{
    /// <summary>Determines whether the status code is 1XX (100-199).</summary>
    public static bool IsInformational(this HttpStatusCode statusCode) => (int)statusCode < 200;

    /// <summary>Determines whether the status code is 2XX (200-299).</summary>
    public static bool IsSuccessful(this HttpStatusCode statusCode) => (int)statusCode >= 200 && (int)statusCode < 300;

    /// <summary>Determines whether the status code is a redirection (301-399).</summary>
    public static bool IsRedirection(this HttpStatusCode statusCode) => (int)statusCode >= 301 && (int)statusCode < 400;

    /// <summary>Determines whether the status code is 4XX (400-499).</summary>
    public static bool IsClientError(this HttpStatusCode statusCode) => (int)statusCode >= 400 && (int)statusCode < 500;

    /// <summary>Determines whether the status code is 5XX (500-599).</summary>
    public static bool IsServerError(this HttpStatusCode statusCode) => (int)statusCode >= 500;
}
