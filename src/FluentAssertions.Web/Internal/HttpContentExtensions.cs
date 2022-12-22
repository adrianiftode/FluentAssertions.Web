using System.Text;

namespace FluentAssertions.Web.Internal;

internal static class HttpContentExtensions
{
    public static async Task<T?> ReadAsAsync<T>(this HttpContent content, ISerializer serializer)
    {
        var model = await ReadAsAsync(content, typeof(T), serializer);
        return (T?)model;
    }

    public static async Task<object?> ReadAsAsync(this HttpContent content, Type modelType, ISerializer serializer)
    {
        var contentStream = await content.ReadAsStreamAsync();
        contentStream.Seek(0, SeekOrigin.Begin);
        var result = await serializer.Deserialize(contentStream, modelType);
        contentStream.Seek(0, SeekOrigin.Begin);
        return result;
    }

    public static Task<T?> ReadAsAsync<T>(this HttpContent content, T _, ISerializer serializer) 
        => content.ReadAsAsync<T>(serializer);

    public static bool IsDisposed(this HttpContent? content)
    {
        if (content == null)
        {
            return true;
        }

        try
        {
            var _ = content?.Headers?.ContentLength;
            return false;
        }
        catch (ObjectDisposedException)
        {
            return true;
        }
    }

    public static bool TryGetContentLength(this HttpContent? content, out long length)
    {
        try
        {
            length = content?.Headers?.ContentLength ?? 0;
            return true;
        }
        catch (Exception)
        {
            length = 0;
            return false;
        }
    }

    public static bool TryGetContentTypeMediaType(this HttpContent? content, out string? mediaType)
    {
        try
        {
            mediaType = content?.Headers?.ContentType?.MediaType;
            return true;
        }
        catch (Exception)
        {
            mediaType = null;
            return false;
        }
    }

    public static async Task<string?> SafeReadAsStringAsync(this HttpContent? content, string? defaultMessage = null)
    {
        if (content == null)
        {
            return null;
        }

        try
        {
            var stream = await content.ReadAsStreamAsync();
            if (stream == null)
            {
                return "";
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            return await sr.ReadToEndAsync();
        }
        catch (ObjectDisposedException)
        {
            return defaultMessage ?? ContentFormatterOptions.WarningMessageWhenDisposed;
        }
    }
}