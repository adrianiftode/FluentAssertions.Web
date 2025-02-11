// ReSharper disable once CheckNamespace

namespace FluentAssertions;

/// <summary>
/// Newtonsoft.Json based serializer
/// </summary>
public class NewtonsoftJsonSerializer : ISerializer
{
    /// <inheritdoc/>
    public Task<object?> Deserialize(Stream content, Type modelType)
    {
        try
        {
            var serializer = JsonSerializer.Create(NewtonsoftJsonSerializerConfig.Options);

            using var sr = new StreamReader(content, Encoding.UTF8, true, 1024, leaveOpen: true);
            using var reader = new JsonTextReader(sr);
            var result = serializer.Deserialize(reader, modelType);
            return Task.FromResult((object?)result);
        }

        catch (JsonException ex)
        {
            throw new DeserializationException($"Exception while deserializing the model with {nameof(NewtonsoftJsonSerializer)}", ex);
        }
    }
}
