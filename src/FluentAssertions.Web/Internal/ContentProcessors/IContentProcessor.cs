using System.Text;

#if AAV
namespace AwesomeAssertions.Web.Internal.ContentProcessors;
#else
namespace FluentAssertions.Web.Internal.ContentProcessors;
#endif

internal interface IContentProcessor
{
    Task GetContentInfo(StringBuilder contentBuilder);
}