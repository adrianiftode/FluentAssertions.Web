// ReSharper disable once CheckNamespace
namespace FluentAssertions
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests.
    /// </summary>
    [DebuggerNonUserCode]
    public static class HttpResponseMessageFluentAssertionsExtensions
    {
        /// <summary>
        /// Returns an <see cref="HttpResponseMessageAssertions"/> object that can be used to assert the
        /// current <see cref="HttpResponseMessage"/>.
        /// </summary>
        public static HttpResponseMessageAssertions Should(this HttpResponseMessage? actual)
            => new HttpResponseMessageAssertions(actual, AssertionChain.GetOrCreate());
    }
}