﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal
{
    internal static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var stringResponse = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponse);
        }

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
}