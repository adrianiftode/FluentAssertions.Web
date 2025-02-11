using System.Text;

namespace Ited.HttpFormatter.Internal.ContentProcessors;

internal interface IContentProcessor
{
    Task GetContentInfo(StringBuilder contentBuilder);
}