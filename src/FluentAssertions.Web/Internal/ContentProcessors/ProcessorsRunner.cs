using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal static class ProcessorsRunner
    {
        public static async Task<StringBuilder> RunProcessors(IEnumerable<IContentProcessor> processors)
        {
            var contentBuilder = new StringBuilder();
            foreach (var processor in processors)
            {
                await processor.GetContentInfo(contentBuilder);
            }

            return contentBuilder;
        }

        public static IReadOnlyCollection<IContentProcessor> CommonProcessors(HttpContent content) => new IContentProcessor[]
        {
            new JsonProcessor(content),
            new BinaryProcessor(content),
            new MultipartProcessor(content),
            new FallbackProcessor(content)
        };
    }
}