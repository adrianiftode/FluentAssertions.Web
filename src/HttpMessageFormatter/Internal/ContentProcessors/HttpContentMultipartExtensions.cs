using Microsoft.AspNetCore.WebUtilities;

namespace HttpMessageFormatter.Internal.ContentProcessors;

internal static class HttpContentMultipartExtensions
{
    public static async Task<IReadOnlyCollection<HttpContent>> ReadAsMultipartExtensionAsync(this HttpContent content)
    {
        var mediaType = content.Headers.ContentType;
        if (mediaType == null) throw new ArgumentException("Missing Content-Type header.");

        string? boundary = null;
        foreach (var parameter in mediaType.Parameters)
        {
            if (parameter.Name.Equals("boundary", StringComparison.OrdinalIgnoreCase))
            {
                boundary = parameter.Value?.Trim('"');
                break;
            }
        }

        if (string.IsNullOrEmpty(boundary)) throw new InvalidDataException("Missing multipart boundary.");

        var parts = new List<HttpContent>();

        using var stream = await content.ReadAsStreamAsync();
        var reader = new MultipartReader(boundary, stream);

        while (await reader.ReadNextSectionAsync() is { } section)
        {
            var memoryStream = new MemoryStream();
            await section.Body.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var partContent = new StreamContent(memoryStream);

            foreach (var header in section.Headers)
            {
                partContent.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            parts.Add(partContent);
        }

        return parts;
    }
}