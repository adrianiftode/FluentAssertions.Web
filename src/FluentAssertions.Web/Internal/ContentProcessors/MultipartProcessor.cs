using System.Linq;
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
            await Handle(contentBuilder);
        }

        private async Task Handle(StringBuilder contentBuilder)
        {
            var multipartContent = (MultipartFormDataContent)_httpContent;
            var boundary = multipartContent.Headers?.ContentType?.Parameters.FirstOrDefault(c => c.Name == "boundary")?.Value?.Trim('\"');

            foreach (var content in multipartContent)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(boundary);
                Appender.AppendHeaders(contentBuilder, content.Headers);
                await Appender.AppendContent(contentBuilder, content);
                contentBuilder.AppendLine();
            }

            contentBuilder.AppendLine();
            contentBuilder.AppendLine($"{boundary}--");


        }


        private bool CanHandle() => _httpContent is MultipartContent;

    }
}