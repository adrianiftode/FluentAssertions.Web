using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal
{
    internal static class HttpContentExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new NullableConverterFactory() },
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        public static async Task<T?> ReadAsAsync<T>(this HttpContent content)
        {
            var contentStream = await content.ReadAsStreamAsync();
            contentStream.Seek(0, SeekOrigin.Begin);
            var result = await JsonSerializer.DeserializeAsync<T>(contentStream, JsonSerializerOptions);
            contentStream.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static async Task<object?> ReadAsAsync(this HttpContent content, Type modelType)
        {
            var contentStream = await content.ReadAsStreamAsync();
            contentStream.Seek(0, SeekOrigin.Begin);
            var result = await JsonSerializer.DeserializeAsync(contentStream, modelType, JsonSerializerOptions);
            contentStream.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public static Task<T?> ReadAsAsync<T>(this HttpContent content, T _) => content.ReadAsAsync<T>();

        public static bool IsDisposed(this HttpContent content)
        {
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

        public static bool TryGetContentLength(this HttpContent content, out long length)
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

        public static async Task<string> SafeReadAsStringAsync(this HttpContent content, string? defaultMessage = null)
        {
            try
            {
                var stream = await content.ReadAsStreamAsync();
                if (stream == null)
                {
                    return "";
                }

                stream.Seek(0, SeekOrigin.Begin);

                using var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
                return await sr.ReadToEndAsync();
            }
            catch (ObjectDisposedException)
            {
                return defaultMessage ?? ContentFormatterOptions.WarningMessageWhenDisposed;
            }
        }
    }

    //https://stackoverflow.com/a/65025191/782754
    internal class NullableConverterFactory : JsonConverterFactory
    {
        static readonly byte[] Empty = Array.Empty<byte>();

        public override bool CanConvert(Type typeToConvert) => Nullable.GetUnderlyingType(typeToConvert) != null;

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(
                typeof(NullableConverter<>).MakeGenericType(
                    new Type[] { Nullable.GetUnderlyingType(type) }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

        class NullableConverter<T> : JsonConverter<T?> where T : struct
        {
            // DO NOT CACHE the return of (JsonConverter<T>)options.GetConverter(typeof(T)) as DoubleConverter.Read() and DoubleConverter.Write()
            // DO NOT WORK for nondefault values of JsonSerializerOptions.NumberHandling which was introduced in .NET 5
            public NullableConverter(JsonSerializerOptions options) { }

            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    if (reader.ValueTextEquals(Empty))
                        return null;
                }

                return JsonSerializer.Deserialize<T>(ref reader, options);
            }

            public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) =>
                JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}