using FluentAssertions.Web.Internal.Serializers;

namespace FluentAssertions.Web.FluentAssertionsWebConfig.Tests.Serializers;

[Collection("Serializers Tests")]
public class SystemTextJsonSerializerTests
{
    [Fact]
    public void DefaultSerializer_IsOfSystemTextJsonSerializer_Type()
    {
        FluentAssertions.FluentAssertionsWebConfig.Serializer.Should().BeOfType<SystemTextJsonSerializer>();
    }

    [Fact]
    public void Options_Is_Not_Null()
    {
        SystemTextJsonSerializerConfig.Options.Should().NotBeNull();
    }
}
