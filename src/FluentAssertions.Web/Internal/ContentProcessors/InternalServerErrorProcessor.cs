using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal class InternalServerErrorProcessor : IContentProcessor
    {
        private readonly HttpResponseMessage _httpResponseMessage;
        private readonly HttpContent _httpContent;
        public InternalServerErrorProcessor(HttpResponseMessage httpResponseMessage, HttpContent httpContent)
        {
            _httpResponseMessage = httpResponseMessage;
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

        private bool CanHandle() =>
            _httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError;

        private async Task Handle(StringBuilder contentBuilder)
        {
            var content = await _httpContent.ReadAsStringAsync();

            AspNetCore30(contentBuilder, content);

            AspNetCore22(contentBuilder, content);
        }

        private static void AspNetCore30(StringBuilder contentBuilder, string content)
        {
            const string headersText = "HEADERS";
            var headersIndex = content.IndexOf(headersText);
            if (headersIndex >= 0)
            {
                var exceptionDetails = content.Substring(0, headersIndex).Trim();
                contentBuilder.Append(exceptionDetails);
            }
        }

        private static void AspNetCore22(StringBuilder contentBuilder, string content)
        {
            const string startTag = @"<pre class=""rawExceptionStackTrace"">";
            if (content.Contains(startTag))
            {
                var startTagIndex = content.LastIndexOf(startTag);
                var endTagIndex = content.IndexOf("</pre>", startTagIndex);
                if (endTagIndex < 0)
                {
                    // there is no end tag
                    return;
                }

                var exceptionDetails = content.Substring(startTagIndex + startTag.Length, endTagIndex - startTagIndex - startTag.Length);

                exceptionDetails = WebUtility.HtmlDecode(exceptionDetails);

                contentBuilder.Append(exceptionDetails);
            }
        }
    }
}
