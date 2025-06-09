using AwesomeAssertions.Web.Internal.Serializers;

namespace AwesomeAssertions.Web.AwesomeAssertionsWebConfig.Tests.Serializers;

[Collection("Serializers Tests")]
public class SystemTextJsonSerializerTests
{
    [Fact]
    public void DefaultSerializer_IsOfSystemTextJsonSerializer_Type()
    {
        AwesomeAssertions.AwesomeAssertionsWebConfig.Serializer.Should().BeOfType<SystemTextJsonSerializer>();
    }

    [Fact]
    public void Options_Is_Not_Null()
    {
        SystemTextJsonSerializerConfig.Options.Should().NotBeNull();
    }
}
