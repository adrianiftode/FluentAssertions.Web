using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class FallbackProcessor : ProcessorBase
    {
        private readonly HttpContent? _httpContent;

        public FallbackProcessor(HttpContent? httpContent)
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

            if (_httpContent!.IsDisposed())
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenDisposed);
                return;
            }

            // we might get here some StreamContent, let's try to print it
            // but let's try not to get into this issue again https://github.com/adrianiftode/FluentAssertions.Web/issues/93
            _httpContent!.TryGetContentLength(out var contentLength);
            var content = await _httpContent!.SafeReadAsStringAsync();
            if (contentLength >= ContentFormatterOptions.MaximumReadableBytes)
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendLine(ContentFormatterOptions.WarningMessageWhenContentIsTooLarge);
                content = content!.Substring(0,ContentFormatterOptions.MaximumReadableBytes);
            }

            contentBuilder.Append(content);
        }
    }
}
