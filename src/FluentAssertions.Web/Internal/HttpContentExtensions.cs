#if AAV
using AwesomeAssertions;
#else
using FluentAssertions;
#endif

#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

/// <summary>
/// Provides extension methods for working with <see cref="HttpContent"/> instances.
/// </summary>
internal static class HttpContentExtensions
{
    /// <summary>
    /// Reads the content of the <see cref="HttpContent"/> as an instance of the specified type.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="serializer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T?> ReadAsAsync<T>(this HttpContent content, ISerializer serializer)
    {
        var model = await ReadAsAsync(content, typeof(T), serializer);
        return (T?)model;
    }

    /// <summary>
    /// Reads the content of the <see cref="HttpContent"/> as an instance of the specified type.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="modelType"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public static async Task<object?> ReadAsAsync(this HttpContent content, Type modelType, ISerializer serializer)
    {
        var contentStream = await content.ReadAsStreamAsync();
        contentStream.Seek(0, SeekOrigin.Begin);

        if (IsTuple(modelType) && await StartsWithJsonObjectAsync(contentStream).ConfigureAwait(false))
        {
            throw new DeserializationException(
                $"A JSON object cannot be deserialized into {modelType} because the names of a tuple's elements, " +
                "such as 'Property' in (string Property, object _), exist only at compile time and are absent from the " +
                "runtime type, which exposes its elements as Item1 and Item2. Deserialize into a type with named " +
                "members, such as a class, a record or a struct, or deserialize a JSON array into the tuple with a " +
                "converter registered for that tuple type.");
        }

        var result = await serializer.Deserialize(contentStream, modelType);
        contentStream.Seek(0, SeekOrigin.Begin);
        return result;
    }

    /// <summary>
    /// Determines whether the runtime type is a tuple, whose element names never survive the compilation.
    /// </summary>
    private static bool IsTuple(Type modelType)
    {
        if (!modelType.IsGenericType)
        {
            return false;
        }

        string? definition = modelType.GetGenericTypeDefinition().FullName;

        return definition != null
            && (definition.StartsWith("System.ValueTuple`", StringComparison.Ordinal)
                || definition.StartsWith("System.Tuple`", StringComparison.Ordinal));
    }

    /// <summary>
    /// Determines whether the content starts a JSON object, leaving the stream position untouched.
    /// </summary>
    /// <remarks>
    /// A JSON array is left to the serializer, which may well support it through a registered converter.
    /// </remarks>
    private static async Task<bool> StartsWithJsonObjectAsync(Stream contentStream)
    {
        long origin = contentStream.Position;

        try
        {
            byte[] buffer = new byte[8];
            int read = await contentStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

            for (int i = 0; i < read; i++)
            {
                byte current = buffer[i];

                if (current == (byte)'{')
                {
                    return true;
                }

                if (current != (byte)' ' && current != (byte)'\t' && current != (byte)'\r' && current != (byte)'\n')
                {
                    return false;
                }
            }

            return false;
        }
        finally
        {
            contentStream.Seek(origin, SeekOrigin.Begin);
        }
    }

    /// <summary>
    /// Reads the content of the <see cref="HttpContent"/> as an instance of the specified type.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="_"></param>
    /// <param name="serializer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<T?> ReadAsAsync<T>(this HttpContent content, T _, ISerializer serializer)
        => content.ReadAsAsync<T>(serializer);
}