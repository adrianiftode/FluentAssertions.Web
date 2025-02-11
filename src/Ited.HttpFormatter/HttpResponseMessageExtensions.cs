﻿namespace Ited.HttpFormatter;

public static class HttpResponseMessageExtensions
{
    public static IEnumerable<string> GetHeaderValues(this HttpResponseMessage response, string header)
    {
        var headers = response.GetHeaders();
        return headers
            .FirstOrDefault(c => string.Equals(c.Key, header, StringComparison.OrdinalIgnoreCase))
            .Value
            .Where(c => !string.IsNullOrEmpty(c));
    }

    public static string? GetFirstHeaderValue(this HttpResponseMessage response, string header)
    {
        var values = response.GetHeaderValues(header);

        return values.FirstOrDefault();
    }

    public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetHeaders(this HttpResponseMessage response)
    {
        var responseContentHeaders =
            response.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>();
        return response.Headers.Union(responseContentHeaders);
    }

    public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetHeaders(this HttpRequestMessage request)
    {
        var requestContentHeaders =
            request.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>();
        return request.Headers.Union(requestContentHeaders);
    }

    public static async Task<string?> GetStringContent(this HttpResponseMessage response)
        => response.Content != null ? await response.Content.ReadAsStringAsync() : null;

    public static async Task<JsonDocument> GetJsonDocument(this HttpResponseMessage response)
    {
        var content = await response.GetStringContent();

        return JsonDocument.Parse(content!);
    }
}