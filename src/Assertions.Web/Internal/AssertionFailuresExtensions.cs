using System.Text;

namespace Assertions.Web.Internal;

internal static class AssertionFailuresExtensions
{
    public static string Format(this AssertionsFailures assertionsFailures)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine();
        messageBuilder.AppendLine();

        foreach (var failure in assertionsFailures.FailuresMessages)
        {
            messageBuilder.AppendLine($"    - {failure.ReplaceFirstWithLowercase()}");
        }

        var fragment = messageBuilder.ToString();
        return fragment;
    }

    public static string Format(this AssertionsFailure assertionsFailure)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine();
        messageBuilder.AppendLine();

        messageBuilder.AppendLine(assertionsFailure.FailureMessage);

        var fragment = messageBuilder.ToString();
        return fragment;
    }
}