using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task<string> GetBeautifiedStringContent(this HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return null;
            }

            string result;
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                result = JToken.Parse(content).ToString();
            }
            catch
            {
                result = content;
            }

            return result;
        }

        public static async Task<string> GetStringContent(this HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<ExpandoObject> GetExpandoContent(this HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return null;
            }

            try
            {
                return await response.Content.ReadAsAsync<ExpandoObject>();
            }
            catch
            {
                return null;
            }
        }
    }
}