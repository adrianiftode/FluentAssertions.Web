namespace Assertions.Web.Internal;

internal class AssertionsFailures(string[]? failuresMessages)
{
    public IReadOnlyCollection<string> FailuresMessages { get; } = failuresMessages ?? Array.Empty<string>();

    public override string ToString() => this.Format();
}

internal class AssertionsFailure(string? failureMessage)
{
    public string? FailureMessage { get; set; } = failureMessage;

    public override string ToString() => this.Format();
}
