#if SH
using Shouldly;
#elif AAV
using AwesomeAssertions;
#else
using FluentAssertions;
#endif

// ReSharper disable CheckNamespace
#pragma warning disable IDE0130
#if SH
namespace Shouldly;
#elif AAV
namespace AwesomeAssertions;
#else
namespace FluentAssertions;
#endif
#pragma warning restore IDE0130

#if SH
/// <summary>
/// Holder of the global <see cref="ShouldlyAssertionsWebConfig"/>
/// </summary>
public static class ShouldlyAssertionsWebConfig
#elif AAV
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

#if SH
    static ShouldlyAssertionsWebConfig() => Serializer = new SystemTextJsonSerializer();
#elif AAV
    static AwesomeAssertionsWebConfig() => Serializer = new SystemTextJsonSerializer();
#else
    static FluentAssertionsWebConfig() => Serializer = new SystemTextJsonSerializer();
#endif

    /// <summary>
    /// The serializer instance used to deserialize the responses into a model of a specified typed
    /// </summary>
    public static ISerializer Serializer
    {
        get => _serializer ?? throw new InvalidOperationException("Serializer cannot be null");

        set => _serializer = value ?? throw new ArgumentNullException(nameof(value), "Serializer cannot be null.");
    }
}
