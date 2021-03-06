﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentAssertions.Web.Internal.ContentProcessors
{
    internal static class Appender
    {
        public static void AppendHeaders(StringBuilder messageBuilder, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            foreach (var header in headers)
            {
                foreach (var headerValue in header.Value)
                {
                    messageBuilder.AppendLine($"{header.Key}: {headerValue}");
                }
            }
        }
        public static async Task AppendContent(StringBuilder contentBuilder, HttpContent content)
        {
            var partContentBuilder = await ProcessorsRunner.RunProcessors(ProcessorsRunner.CommonProcessors(content));

            if (partContentBuilder.Length > 0)
            {
                contentBuilder.AppendLine();
                contentBuilder.Append(partContentBuilder);
            }
        }
    }
}