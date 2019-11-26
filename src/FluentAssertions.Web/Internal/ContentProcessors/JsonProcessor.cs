using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{

    internal class JsonProcessor : IContentProcessor
    {
        private readonly HttpContent _httpContent;
        public JsonProcessor(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        public async Task GetContentInfo(StringBuilder contentBuilder)
        {
            if (!CanHandle())
            {
                return;
            }

            var content = await _httpContent.SafeReadAsStringAsync();

            var beautified = content?.BeautifyJson();
            if (!string.IsNullOrEmpty(beautified))
            {
                contentBuilder.Append(beautified);
            }
        }

        private bool CanHandle()
        {
            if (_httpContent == null)
            {
                return false;
            }

            _httpContent.TryGetContentLength(out long length);
            return length <= ContentFormatterOptions.MaximumReadableBytes
                   && _httpContent.Headers.ContentType?.MediaType == "application/json";
        }
    }
}
