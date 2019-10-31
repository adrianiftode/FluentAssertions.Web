using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class MultipartProcessor : IContentProcessor
    {
        private readonly HttpContent _httpContent;
        public MultipartProcessor(HttpContent httpContent)
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

        private bool CanHandle() => _httpContent is MultipartContent;

        private Task Handle() => Task.CompletedTask;
    }
}