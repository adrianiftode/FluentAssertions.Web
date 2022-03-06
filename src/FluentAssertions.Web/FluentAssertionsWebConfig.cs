// ReSharper disable once CheckNamespace
using FluentAssertions.Web.Internal.Serializers;
using System;

namespace FluentAssertions
{
    /// <summary>
    /// Holder of the global <see cref="FluentAssertionsWebConfig"/>
    /// </summary>
    public static class FluentAssertionsWebConfig
    {
        private static ISerializer? serializer;
        static FluentAssertionsWebConfig()
        {
            Serializer = new SystemTextJsonSerializer();
        }

        /// <summary>
        /// The serializer instance used to deserialize the responses into a model of a specified typed
        /// </summary>
        public static ISerializer? Serializer
        {
            get => serializer;
            set
            {
                serializer = value ?? throw new ArgumentNullException(nameof(value), "Serializer cannot be null.");
            }
        }
    }
}
