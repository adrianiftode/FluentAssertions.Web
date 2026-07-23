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
        var result = await serializer.Deserialize(contentStream, modelType);
        contentStream.Seek(0, SeekOrigin.Begin);
        return result;
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