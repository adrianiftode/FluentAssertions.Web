using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class MultipartProcessor : ProcessorBase
    {
        private readonly HttpContent? _httpContent;
        public MultipartProcessor(HttpContent? httpContent)
        {
            _httpContent = httpContent;
        }

        protected override async Task Handle(StringBuilder contentBuilder)
        {
            var multipartContent = (MultipartFormDataContent)_httpContent!;
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

        protected override bool CanHandle() => _httpContent is MultipartContent;
    }
}