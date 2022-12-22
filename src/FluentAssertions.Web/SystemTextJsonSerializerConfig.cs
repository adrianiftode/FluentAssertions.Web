// ReSharper disable once CheckNamespace
using FluentAssertions.Web.Internal.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentAssertions;

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
        Converters = { new JsonStringEnumConverter(), new NullableConverterFactory(), new EmptyArrayToObjectConverterFactory() },
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}
