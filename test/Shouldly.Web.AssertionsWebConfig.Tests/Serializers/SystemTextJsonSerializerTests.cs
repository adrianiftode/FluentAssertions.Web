using Assertions.Web.Internal.Serializers;
using Shouldly;

namespace Shouldly.Web.Tests.Serializers;

[Collection("Serializers Tests")]
public class SystemTextJsonSerializerTests
{
    [Fact]
    public void DefaultSerializer_IsOfSystemTextJsonSerializer_Type()
    {
        AssertionsWebConfig.Serializer.ShouldBeOfType<SystemTextJsonSerializer>();
    }

    [Fact]
    public void Options_Is_Not_Null()
    {
        SystemTextJsonSerializerConfig.Options.ShouldNotBeNull();
    }
}