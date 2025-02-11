using FluentAssertions.Formatting;

internal class HttpResponseMessageFormatter : IValueFormatter
{
    public bool CanHandle(object value) => value is HttpResponseMessage;

    /// <inheritdoc />
    public void Format(object value,
        FormattedObjectGraph formattedGraph,
        FormattingContext context,
        FormatChild formatChild)
    {
        var response = (HttpResponseMessage)value;

        var formatted = HttpResponseMessageFormatted.GetFormatted(response);

        formattedGraph.AddFragment(formatted);
    }
}