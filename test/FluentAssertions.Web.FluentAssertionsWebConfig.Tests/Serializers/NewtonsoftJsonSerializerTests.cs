using System;
using Xunit;

namespace FluentAssertions.Web.FluentAssertionsWebConfig.Tests
{
    [Collection("Serializers Tests")]
    public sealed class NewtonsoftJsonSerializerTests : IDisposable
    {
        private readonly ISerializer _initialSerializer;
        public NewtonsoftJsonSerializerTests()
        {
            _initialSerializer = FluentAssertions.FluentAssertionsWebConfig.Serializer;
        }
        [Fact]
        public void Serializer_Can_Be_Set_To_NewtonsoftJsonDeserializer()
        {
            FluentAssertions.FluentAssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();

            FluentAssertions.FluentAssertionsWebConfig.Serializer.Should().BeOfType<NewtonsoftJsonSerializer>();
        }

        [Fact]
        public void Options_Is_Not_Null()
        {
            NewtonsoftJsonSerializerConfig.Options.Should().NotBeNull();
        }

        public void Dispose()
        {
            FluentAssertions.FluentAssertionsWebConfig.Serializer = _initialSerializer;
        }
    }
}
