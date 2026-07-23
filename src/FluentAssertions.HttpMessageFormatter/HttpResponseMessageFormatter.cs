#if AAV
using AwesomeAssertions.Formatting;
#else
using FluentAssertions.Formatting;
#endif

#if AAV
namespace AwesomeAssertions.Formatting;
#else
namespace FluentAssertions.Formatting;
#endif

/// <summary>
/// FluentAssertions formatter for HTTP response messages using HttpMessageFormatter.
/// </summary>
public class HttpResponseMessageFormatter : IValueFormatter
{
    public bool CanHandle(object value) => value is HttpResponseMessage;

    public void Format(object value,
        FormattedObjectGraph formattedGraph,
        FormattingContext context,
        FormatChild formatChild)
    {
        var response = (HttpResponseMessage)value;
        var formatted = global::HttpMessageFormatter.HttpResponseFormatterExtensions.Format(response);
        formattedGraph.AddFragment(formatted);
    }
}