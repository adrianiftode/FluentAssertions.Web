#if AAV
using AwesomeAssertions.Formatting;
#else
using FluentAssertions.Formatting;
#endif
using System.Text;

#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

internal class AssertionsFailuresFormatter : IValueFormatter
{
    public bool CanHandle(object value) => value is AssertionsFailures;

    public void Format(object value,
        FormattedObjectGraph formattedGraph,
        FormattingContext context,
        FormatChild formatChild)
    {
        var assertionsFailures = (AssertionsFailures)value;

        var fragment = assertionsFailures.Format();
        formattedGraph.AddFragment(fragment);
    }
}
