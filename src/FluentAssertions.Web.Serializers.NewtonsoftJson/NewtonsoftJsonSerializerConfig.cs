// ReSharper disable once CheckNamespace
using Newtonsoft.Json;

namespace FluentAssertions
{
    /// <summary>
    /// Holder of the global <see cref="JsonSerializerOptions"/>
    /// </summary>
    public static class NewtonsoftJsonSerializerConfig
    {
        /// <summary>
        /// The options used to deserialize a JSON into a C# object
        /// </summary>
        public static readonly JsonSerializerSettings Options = new();
    }
}
