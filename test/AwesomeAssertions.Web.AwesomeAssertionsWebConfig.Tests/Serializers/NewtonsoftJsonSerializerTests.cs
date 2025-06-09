namespace AwesomeAssertions.Web.AwesomeAssertionsWebConfig.Tests.Serializers;

[Collection("Serializers Tests")]
public sealed class NewtonsoftJsonSerializerTests : IDisposable
{
    private readonly ISerializer _initialSerializer;
    public NewtonsoftJsonSerializerTests()
    {
        _initialSerializer = AwesomeAssertions.AwesomeAssertionsWebConfig.Serializer;
    }
    [Fact]
    public void Serializer_Can_Be_Set_To_NewtonsoftJsonDeserializer()
    {
        AwesomeAssertions.AwesomeAssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();

        AwesomeAssertions.AwesomeAssertionsWebConfig.Serializer.Should().BeOfType<NewtonsoftJsonSerializer>();
    }

    [Fact]
    public void Options_Is_Not_Null()
    {
        NewtonsoftJsonSerializerConfig.Options.Should().NotBeNull();
    }

    public void Dispose()
    {
        AwesomeAssertions.AwesomeAssertionsWebConfig.Serializer = _initialSerializer;
    }
}
