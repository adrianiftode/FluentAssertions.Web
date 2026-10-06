using HttpMessageFormatter;

// ReSharper disable once CheckNamespace
#if AAV
namespace AwesomeAssertions;
#else
namespace FluentAssertions;
#endif

#if AAV
/// <summary>
/// Holder of the global <see cref="AwesomeAssertionsWebConfig"/>
/// </summary>
public static class AwesomeAssertionsWebConfig
#else
/// <summary>
/// Holder of the global <see cref="FluentAssertionsWebConfig"/>
/// </summary>
public static class FluentAssertionsWebConfig
#endif
{
    private static ISerializer? _serializer;
    private static HttpResponseFormatterOptions? _responseFormatterOptions;

#if AAV
    static AwesomeAssertionsWebConfig()
    {
        Serializer = new SystemTextJsonSerializer();
        ResponseFormatterOptions = new HttpResponseFormatterOptions();
    }
#else
    static FluentAssertionsWebConfig()
    {
        Serializer = new SystemTextJsonSerializer();
        ResponseFormatterOptions = new HttpResponseFormatterOptions();
    }
#endif

    /// <summary>
    /// The serializer instance used to deserialize the responses into a model of a specified typed
    /// </summary>
    public static ISerializer Serializer
    {
        get => _serializer ?? throw new InvalidOperationException("Serializer cannot be null");

        set => _serializer = value ?? throw new ArgumentNullException(nameof(value), "Serializer cannot be null.");
    }

    /// <summary>
    /// The options used when formatting an HTTP response message into a readable string,
    /// for example in the assertion failure messages.
    /// They control, for instance, how many characters of the content are printed before the content
    /// is considered too large. These options do not change the assertions themselves, only the way
    /// the HTTP messages are formatted.
    /// </summary>
    public static HttpResponseFormatterOptions ResponseFormatterOptions
    {
        get => _responseFormatterOptions ?? throw new InvalidOperationException("ResponseFormatterOptions cannot be null");

        set => _responseFormatterOptions = value ?? throw new ArgumentNullException(nameof(value), "ResponseFormatterOptions cannot be null.");
    }
}
