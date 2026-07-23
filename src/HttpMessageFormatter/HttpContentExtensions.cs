using HttpMessageFormatter.Internal;

namespace HttpMessageFormatter;

/// <summary>
/// Provides extension methods for working with <see cref="HttpContent"/> instances.
/// </summary>
public static class HttpContentExtensions
{
    /// <summary>
    /// Determines whether the specified <see cref="HttpContent"/> is disposed.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to get the content length of the specified <see cref="HttpContent"/>.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="length"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to get the content type media type of the specified <see cref="HttpContent"/>.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="mediaType"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Attempts to read as string the specified <see cref="HttpContent"/>.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="defaultMessage"></param>
    /// <returns></returns>
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
