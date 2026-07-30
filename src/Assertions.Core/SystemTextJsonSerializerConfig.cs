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

/// <summary>
/// Holder of the global <see cref="System.Text.Json.JsonSerializerOptions"/>
/// </summary>
public static class SystemTextJsonSerializerConfig
{
    /// <summary>
    /// The options used to deserialize a JSON into a C# object
    /// </summary>
    public static readonly JsonSerializerOptions Options = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(), new NullableConverterFactory() },
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}
