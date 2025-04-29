namespace FluentAssertions;

/// <summary>
/// Holder of the global <see cref="ShouldlyWebConfig"/>
/// </summary>
public static class ShouldlyWebConfig
{
    private static ISerializer? _serializer;

    static ShouldlyWebConfig() => Serializer = new SystemTextJsonSerializer();

    /// <summary>
    /// The serializer instance used to deserialize the responses into a model of a specified typed
    /// </summary>
    public static ISerializer Serializer
    {
        get => _serializer ?? throw new InvalidOperationException("Serializer cannot be null");

        set => _serializer = value ?? throw new ArgumentNullException(nameof(value), "Serializer cannot be null.");
    }
}
