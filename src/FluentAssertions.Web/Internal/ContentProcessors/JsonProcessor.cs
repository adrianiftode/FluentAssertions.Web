using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class JsonProcessor : ProcessorBase
    {
        private readonly HttpContent _httpContent;
        public JsonProcessor(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        protected override async Task Handle(StringBuilder contentBuilder)
        {
            var content = await _httpContent.SafeReadAsStringAsync();

            var beautified = content?.BeautifyJson();
            if (!string.IsNullOrEmpty(beautified))
            {
                contentBuilder.Append(beautified);
            }
        }

        protected override bool CanHandle()
        {
            if (_httpContent == null)
            {
                return false;
            }

            _httpContent.TryGetContentLength(out long length);
            var mediaType = _httpContent.Headers.ContentType?.MediaType;
            return length <= ContentFormatterOptions.MaximumReadableBytes
                   && (mediaType.EqualsCaseInsensitive("application/json") || mediaType.EqualsCaseInsensitive("application/problem+json"));
        }
    }
}
