using System.Net.Http;

namespace FluentAssertions.Web
{
    public static class HttpResponseMessageFluentAssertionsExtensions
    {
        public static HttpResponseMessageAssertions Should(this HttpResponseMessage actual)
            => new HttpResponseMessageAssertions(actual);
    }
}