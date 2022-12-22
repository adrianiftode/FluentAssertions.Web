using System.Text;

namespace FluentAssertions.Web.Internal.ContentProcessors;

internal interface IContentProcessor
{
    Task GetContentInfo(StringBuilder contentBuilder);
}