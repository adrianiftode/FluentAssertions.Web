using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class FallbackProcessor : IContentProcessor
    {
        private readonly HttpResponseMessage _httpResponseMessage;
        private readonly HttpContent _httpContent;
        public FallbackProcessor(HttpResponseMessage httpResponseMessage, HttpContent httpContent)
        {
            _httpResponseMessage = httpResponseMessage;
            _httpContent = httpContent;
        }

        public async Task GetContentInfo(StringBuilder contentBuilder)
        {
            if (contentBuilder.Length != 0 || _httpContent == null)
            {
                return;
            }

            if (_httpContent.Headers.ContentLength >= ContentFormatterOptions.MaximumReadableBytes)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine("Content is too large to display.");
                return;
            }

            var content = await _httpContent.ReadAsStringAsync();
            contentBuilder.Append(content);
        }
    }
}
