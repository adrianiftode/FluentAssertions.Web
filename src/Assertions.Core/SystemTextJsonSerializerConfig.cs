
using System.Text.Json;
using System.Text.Json.Serialization;
using Assertions.Core.Internal.Serializers;

// ReSharper disable once CheckNamespace
#if SH
namespace Shouldly;
#elif AAV
namespace AwesomeAssertions;
#else
namespace FluentAssertions;
#endif

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
