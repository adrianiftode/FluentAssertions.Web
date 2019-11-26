using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class FallbackProcessor : IContentProcessor
    {
        private readonly HttpContent _httpContent;
        public FallbackProcessor(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        public async Task GetContentInfo(StringBuilder contentBuilder)
        {
            if (contentBuilder.Length != 0 || _httpContent == null)
            {
                return;
            }

            if (_httpContent.IsDisposed())
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenDisposed);
                return;
            }

            var contentLength = _httpContent.Headers.ContentLength;
            if (contentLength >= ContentFormatterOptions.MaximumReadableBytes)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenContentIsTooLarge);
                return;
            }

            var content = await _httpContent.SafeReadAsStringAsync();
            contentBuilder.Append(content);
        }
    }
}
