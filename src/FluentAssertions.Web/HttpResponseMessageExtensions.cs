using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace FluentAssertions;

/// <summary>
///
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Returns an <see cref="HttpResponseMessageAssertions"/> object that can be used to assert the
    /// current <see cref="HttpResponseMessage"/>.
    /// </summary>
    [Pure]
    public static HttpResponseMessageAssertions Should([NotNull] this HttpResponseMessage actualValue)
    {
        return new HttpResponseMessageAssertions(actualValue, AssertionChain.GetOrCreate());
    }
}
