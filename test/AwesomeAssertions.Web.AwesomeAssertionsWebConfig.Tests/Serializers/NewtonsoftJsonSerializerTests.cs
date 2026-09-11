namespace AwesomeAssertions.Web.AwesomeAssertionsWebConfig.Tests.Serializers;

[Collection("Serializers Tests")]
public sealed class NewtonsoftJsonSerializerTests : IDisposable
{
    private readonly ISerializer _initialSerializer;
    public NewtonsoftJsonSerializerTests()
    {
        _initialSerializer = AssertionsWebConfig.Serializer;
    }
    [Fact]
    public void Serializer_Can_Be_Set_To_NewtonsoftJsonDeserializer()
    {
        AssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();

        AssertionsWebConfig.Serializer.Should().BeOfType<NewtonsoftJsonSerializer>();
    }

    [Fact]
    public void Options_Is_Not_Null()
    {
        NewtonsoftJsonSerializerConfig.Options.Should().NotBeNull();
    }

    public void Dispose()
    {
        AssertionsWebConfig.Serializer = _initialSerializer;
    }
}
