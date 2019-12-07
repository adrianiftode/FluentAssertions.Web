using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal abstract class ProcessorBase : IContentProcessor
    {
        public async Task GetContentInfo(StringBuilder contentBuilder)
        {
            if (!CanHandle())
            {
                return;
            }

            await Handle(contentBuilder);
        }

        protected abstract bool CanHandle();
        protected abstract Task Handle(StringBuilder contentBuilder);
    }
}