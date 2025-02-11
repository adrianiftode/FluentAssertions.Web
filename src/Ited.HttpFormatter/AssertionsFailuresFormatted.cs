using System.Text;

namespace Ited.HttpFormatter;

public class AssertionsFailuresFormatted
{
    public static string GetFormatted(AssertionsFailures assertionsFailures)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine();
        messageBuilder.AppendLine();

        foreach (var failure in assertionsFailures.FailuresMessages)
        {
            messageBuilder.AppendLine($"    - {failure.ReplaceFirstWithLowercase()}");
        }
        return messageBuilder.ToString();
    }
}
