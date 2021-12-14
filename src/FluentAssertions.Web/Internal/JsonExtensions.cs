using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace FluentAssertions.Web.Internal
{
    internal static class JsonExtensions
    {
        public static IEnumerable<string> GetStringValuesOf(this JsonDocument json, string propertyName)
        {
            var byKey = json.GetPropertiesByName(propertyName);

            return GetStringValues(byKey);
        }

        public static IEnumerable<string> GetStringValuesOf(this JsonProperty property, string propertyName)
        {
            var byKey = property.GetPropertiesByName(propertyName);

            return GetStringValues(byKey);
        }

        public static IEnumerable<string> GetChildrenNames(this JsonDocument json, string? parentElement)
        {
            if (string.IsNullOrEmpty(parentElement))
            {
                return json.RootElement.AllProperties().Select(c => c.Name);
            }

            return json.GetPropertiesByName(parentElement!)
                .SelectMany(c => c.Value.AllProperties().Select(p => p.Name));
        }

        public static string? GetParentKey(this JsonDocument json, string childElement)
            => GetByKeyWithParent(json, childElement)
                .Select(c => c.parent?.Name).FirstOrDefault();

        public static IEnumerable<JsonProperty> GetPropertiesByName(this JsonDocument json, string propertyName)
            => json.RootElement.AllProperties().Where(property => string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase));

        public static IEnumerable<JsonProperty> GetPropertiesByName(this JsonProperty property, string propertyName)
            => property.Value.AllProperties().Where(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

        private static IEnumerable<(JsonProperty element, JsonProperty? parent)> GetByKeyWithParent(this JsonDocument json, string key)
            => json.RootElement.AllPropertiesWithParent().Where(pair => string.Equals(pair.property.Name, key, StringComparison.OrdinalIgnoreCase));

        private static IEnumerable<JsonProperty> AllProperties(this JsonElement obj)
            => obj.AllPropertiesWithParent().Select(pair => pair.property);

        private static IEnumerable<string> GetStringValues(IEnumerable<JsonProperty> properties)
        {
            if (!properties.Any())
            {
                return Enumerable.Empty<string>();
            }

            var first = properties.First();
            switch (first.Value.ValueKind)
            {
                case JsonValueKind.String:
                    return new[] { first.Value.GetString() ?? "" };
                case JsonValueKind.Array:
                    return first.Value.EnumerateArray().Where(c => c.ValueKind == JsonValueKind.String).Select(c => c.GetString() ?? "");
                default:
                    return Enumerable.Empty<string>();
            }
        }


        private static IEnumerable<(JsonProperty property, JsonProperty? parent)> AllPropertiesWithParent(this JsonElement obj)
        {
            var toSearch = new Stack<(JsonElement element, JsonProperty? parent)>();
            toSearch.Push((obj, default));

            while (toSearch.Count > 0)
            {
                var inspected = toSearch.Pop();

                if (inspected.element.ValueKind == JsonValueKind.Array)
                {
                    foreach (var arrayElement in inspected.element.EnumerateArray())
                    {
                        if (arrayElement.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var property in arrayElement.EnumerateObject())
                            {
                                toSearch.Push((property.Value, inspected.parent));
                            }
                        }
                    }
                }
                else if (inspected.element.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in inspected.element.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Object)
                        {
                            toSearch.Push((property.Value, property));
                        }

                        yield return (property, inspected.parent);
                    }
                }
            }
        }
    }
}
