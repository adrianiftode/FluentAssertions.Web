namespace Assertions.Core.Internal;

internal class AssertionsFailures(string[]? failuresMessages)
{
    public IReadOnlyCollection<string> FailuresMessages { get; } = failuresMessages ?? Array.Empty<string>();
}

internal class AssertionsFailure(string? failureMessage)
{
    public string? FailureMessage { get; set; } = failureMessage;
}
