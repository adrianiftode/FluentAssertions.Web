using System.Text.Json;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
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
