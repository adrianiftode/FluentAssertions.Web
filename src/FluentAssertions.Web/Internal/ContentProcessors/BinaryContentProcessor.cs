using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class StreamProcessor : BinaryContentProcessor
    {
        private static readonly Func<HttpContent, bool> CanHandleFunc = content =>
                    content is StreamContent
                    && ((IsApplicationOctetStream(content) ?? true)
                        || (IsBinaryImage(content) ?? true));
        public StreamProcessor(HttpContent httpContent)
            : base(httpContent, CanHandleFunc, "stream")
        {
        }
    }

    internal class ByteArrayContentProcessor : BinaryContentProcessor
    {
        private static readonly Func<HttpContent, bool> CanHandleFunc = content =>
                    content is ByteArrayContent
                    && !(content is StringContent || content is FormUrlEncodedContent)
                    && ((IsApplicationOctetStream(content) ?? true)
                        || (IsBinaryImage(content) ?? true));

        public ByteArrayContentProcessor(HttpContent httpContent)
            : base(httpContent, CanHandleFunc, "binary data")
        {
        }
    }

    internal class BinaryContentProcessor : ProcessorBase
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

        protected override Task Handle(StringBuilder contentBuilder)
        {
            var contentLength = _httpContent.Headers?.ContentLength ?? (_httpContent.TryGetContentLength(out var length) ? length : 0);

            contentBuilder.AppendLine(string.Format(ContentFormatterOptions.ContentIsSomeTypeHavingLength, _dataType, contentLength));

            return Task.CompletedTask;
        }

        protected override bool CanHandle() => _canHandlePredicate(_httpContent);

        protected static bool? IsApplicationOctetStream(HttpContent? content)
        {
            var contentTypeMediaType = content?.Headers?.ContentType?.MediaType;
            if (contentTypeMediaType == null)
            {
                return default;
            }

            return contentTypeMediaType.Contains("application/")
                && !(contentTypeMediaType.Contains("xml") || contentTypeMediaType.Contains("json"));
        }

        protected static bool? IsBinaryImage(HttpContent? content)
        {
            var contentTypeMediaType = content?.Headers?.ContentType?.MediaType;
            if (contentTypeMediaType == null)
            {
                return default;
            }

            return contentTypeMediaType.Contains("image/")
                   && !(contentTypeMediaType.EndsWith("xml") || contentTypeMediaType.EndsWith("webp"));
        }
    }
}
