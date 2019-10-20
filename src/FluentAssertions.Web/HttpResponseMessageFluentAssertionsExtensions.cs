using FluentAssertions.Web;
using System.Net.Http;

namespace FluentAssertions
{
    public static class HttpResponseMessageFluentAssertionsExtensions
    {
        public static HttpResponseMessageAssertions Should(this HttpResponseMessage actual)
            => new HttpResponseMessageAssertions(actual);
    }
}