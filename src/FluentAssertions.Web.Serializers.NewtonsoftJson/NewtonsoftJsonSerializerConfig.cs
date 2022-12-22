// ReSharper disable once CheckNamespace
namespace FluentAssertions;

/// <summary>
/// Holder of the global <see cref="Newtonsoft.Json.JsonSerializerSettings"/>
/// </summary>
public static class NewtonsoftJsonSerializerConfig
{
    /// <summary>
    /// The options used to deserialize a JSON into a C# object
    /// </summary>
    public static readonly JsonSerializerSettings Options = new();
}
