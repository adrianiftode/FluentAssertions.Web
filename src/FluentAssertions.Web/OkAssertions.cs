using System.Net.Http;

namespace FluentAssertions.Web
{
    public class OkAssertions : HttpResponseMessageAssertions
    {
        public OkAssertions(HttpResponseMessage value) : base(value)
        {

        }
    }
}