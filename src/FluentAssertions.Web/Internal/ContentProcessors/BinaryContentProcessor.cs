using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class StreamProcessor : BinaryContentProcessor
    {
        public StreamProcessor(HttpContent httpContent)
            : base(httpContent, content => content is StreamContent, "stream")
        {
        }
    }

    internal class ByteArrayContentProcessor : BinaryContentProcessor
    {
        public ByteArrayContentProcessor(HttpContent httpContent)
            : base(httpContent,
                content => content is ByteArrayContent && !(content is StringContent || content is FormUrlEncodedContent),
                "binary data")
        {
        }
    }

    internal class BinaryContentProcessor : IContentProcessor
    {
        private readonly HttpContent _httpContent;
        private readonly Func<HttpContent, bool> _canHandlePredicate;
        private readonly string _dataType;

        public BinaryContentProcessor(HttpContent httpContent, Func<HttpContent, bool> canHandlePredicate, string dataType)
        {
            _httpContent = httpContent;
            _canHandlePredicate = canHandlePredicate;
            _dataType = dataType;
        }

        public async Task GetContentInfo(StringBuilder contentBuilder)
        {
            if (!CanHandle())
            {
                return;
            }
            await Handle(contentBuilder);
        }

        private Task Handle(StringBuilder contentBuilder)
        {
            var contentLength = _httpContent?.Headers.ContentLength ?? (_httpContent.TryGetContentLength(out var length) ? length : 0);

            contentBuilder.AppendLine(string.Format(ContentFormatterOptions.ContentIsSomeTypeHavingLength, _dataType, contentLength));

            return Task.CompletedTask;
        }

        private bool CanHandle()
            => _canHandlePredicate(_httpContent);
    }
}
