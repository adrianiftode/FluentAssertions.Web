using FluentAssertions.Web.Internal.Serializers;
using System;

// ReSharper disable once CheckNamespace
namespace FluentAssertions
{
    /// <summary>
    /// Holder of the global <see cref="FluentAssertionsWebConfig"/>
    /// </summary>
    public static class FluentAssertionsWebConfig
    {
        private static ISerializer? _serializer;

        static FluentAssertionsWebConfig() => Serializer = new SystemTextJsonSerializer();

        /// <summary>
        /// The serializer instance used to deserialize the responses into a model of a specified typed
        /// </summary>
        public static ISerializer Serializer
        {
            get => _serializer ?? throw new InvalidOperationException("Serializer cannot be null");

            set => _serializer = value ?? throw new ArgumentNullException(nameof(value), "Serializer cannot be null.");
        }
    }
}
