using FluentAssertions.Formatting;

namespace FluentAssertions.Web.Internal;

internal class AssertionsFailuresFormatter : IValueFormatter
{
    public bool CanHandle(object value) => value is AssertionsFailures;

    public void Format(object value,
        FormattedObjectGraph formattedGraph,
        FormattingContext context,
        FormatChild formatChild)
    {
        var assertionsFailures = (AssertionsFailures)value;

        var formatted = AssertionsFailuresFormatted.GetFormatted(assertionsFailures);

        formattedGraph.AddFragment(formatted);
    }
}
