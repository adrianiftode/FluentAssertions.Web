using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class FallbackProcessor : ProcessorBase
    {
        private readonly HttpContent _httpContent;

        public FallbackProcessor(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        protected override bool CanHandle() => _httpContent != null;

        protected override async Task Handle(StringBuilder contentBuilder)
        {
            if (contentBuilder.Length > 0)
            {
                return;
            }

            if (_httpContent.IsDisposed())
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenDisposed);
                return;
            }

            var content = await _httpContent.SafeReadAsStringAsync();
            var contentLength = _httpContent.Headers.ContentLength;
            if (contentLength >= ContentFormatterOptions.MaximumReadableBytes)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenContentIsTooLarge);
                content = content.Substring(ContentFormatterOptions.MaximumPrintableBytes);
            }

            contentBuilder.Append(content);
        }
    }
}
