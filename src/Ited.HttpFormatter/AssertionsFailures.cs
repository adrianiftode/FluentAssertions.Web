namespace Ited.HttpFormatter;

public class AssertionsFailures
{
    public AssertionsFailures(string[] failuresMessages)
    {
        FailuresMessages = failuresMessages;
    }
    public IReadOnlyCollection<string> FailuresMessages { get; }
}
