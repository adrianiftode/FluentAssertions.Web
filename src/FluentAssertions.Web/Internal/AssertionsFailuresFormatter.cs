using FluentAssertions.Formatting;
using System.Text;

namespace FluentAssertions.Web.Internal
{
    internal class AssertionsFailuresFormatter : IValueFormatter
    {
        public bool CanHandle(object value) => value is AssertionsFailures;

        public void Format(object value,
            FormattedObjectGraph formattedGraph,
            FormattingContext context,
            FormatChild formatChild)
        {
            var assertionsFailures = (AssertionsFailures)value;

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine();

            foreach (var failure in assertionsFailures.FailuresMessages)
            {
                messageBuilder.AppendLine($"    - { failure.ReplaceFirstWithLowercase() }");
            }

            formattedGraph.AddFragment(messageBuilder.ToString());
        }
    }
}
