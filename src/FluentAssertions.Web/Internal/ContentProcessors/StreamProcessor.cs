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

        private bool CanHandle() => _httpContent is StreamContent;

        private Task Handle() => Task.CompletedTask;
    }
}
