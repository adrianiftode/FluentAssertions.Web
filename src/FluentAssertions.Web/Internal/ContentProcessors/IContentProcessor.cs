using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors;

internal interface IContentProcessor
{
    Task GetContentInfo(StringBuilder contentBuilder);
}