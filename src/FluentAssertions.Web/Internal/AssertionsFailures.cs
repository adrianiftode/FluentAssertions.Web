namespace FluentAssertions.Web.Internal;

internal class AssertionsFailures
{
    public AssertionsFailures(string[] failuresMessages)
    {
        FailuresMessages = failuresMessages;
    }
    public IReadOnlyCollection<string> FailuresMessages { get; }
}
