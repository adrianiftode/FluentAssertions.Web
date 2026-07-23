namespace HttpMessageFormatter.Internal.ContentProcessors;

internal interface IContentProcessor
{
    Task GetContentInfo(StringBuilder contentBuilder);
}
