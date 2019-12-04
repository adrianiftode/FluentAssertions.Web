using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssertions.Web.Internal
{
    internal static class JsonExtensions
    {
        public static bool HasKey(this JObject json, string key)
            => json.GetByKey(key).Any();

        public static IEnumerable<string> GetStringValuesByKey(this JObject json, string key)
        {
            var byKey = json.GetByKey(key);
            if (byKey != null && !byKey.Any())
            {
                return Enumerable.Empty<string>();
            }

            var first = byKey.FirstOrDefault()?.First;
            return first switch
            {
                JArray values => values.Select(c => (string)c),
                { } token => new[] { token.ToString() },
                _ => Enumerable.Empty<string>()
            };
        }

        public static IEnumerable<string> GetChildrenKeys(this JObject json, string? parentElement)
        {
            if (string.IsNullOrEmpty(parentElement))
            {
                return json
                    .Children()
                    .OfType<JProperty>()
                    .Select(c => c.Name);
            }

            return json.GetByKey(parentElement!)
                .SelectMany(c => c.Children<JObject>())
                .SelectMany(c => c.Properties().Select(p => p.Name));
        }

        public static string? GetParentKey(this JObject json, string childElement) =>
            (json.GetByKey(childElement)
                .FirstOrDefault()
                ?.Parent // JObject
                .Parent as JProperty)
            ?.Name;

        public static IEnumerable<JToken> GetByKey(this JObject json, string key) =>
            json.AllTokens().Where(c => c.Type == JTokenType.Property
                                        && string.Equals(((JProperty)c).Name, key, StringComparison.OrdinalIgnoreCase));

        private static IEnumerable<JToken> AllTokens(this JObject obj)
        {
            var toSearch = new Stack<JToken>(obj.Children());
            while (toSearch.Count > 0)
            {
                var inspected = toSearch.Pop();
                yield return inspected;
                foreach (var child in inspected)
                {
                    toSearch.Push(child);
                }
            }
        }
    }
}
