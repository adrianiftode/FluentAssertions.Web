using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class StreamProcessor : IContentProcessor
    {
        private readonly HttpContent _httpContent;
        public StreamProcessor(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        public async Task GetContentInfo(StringBuilder contentBuilder)
        {
            if (!CanHandle())
            {
                return;
            }
            await Handle();
        }

        private bool CanHandle()
        {
            _httpContent.TryGetContentLength(out var length);
            return _httpContent is StreamContent
                   && length < ContentFormatterOptions.MaximumReadableBytes;
        }

        private Task Handle() => Task.CompletedTask;
    }
}
