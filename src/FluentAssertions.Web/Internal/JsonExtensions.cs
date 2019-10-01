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
            => json.GetByKey(key).FirstOrDefault()?.First.Select(c => (string)c) ?? Enumerable.Empty<string>();

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
