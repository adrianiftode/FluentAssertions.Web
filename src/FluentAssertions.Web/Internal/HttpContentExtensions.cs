using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal
{
    internal static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var stringResponse = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponse);
        }
    }
}