namespace Assertions.Web.Tests.Internal;

[Collection("Assertions Web Config Tests")]
public sealed class AssertionsWebConfigTests : IDisposable
{
    private readonly ISerializer _initialSerializer;
    private readonly HttpResponseFormatterOptions _initialResponseFormatterOptions;

    public AssertionsWebConfigTests()
    {
        _initialSerializer = AssertionsWebConfig.Serializer;
        _initialResponseFormatterOptions = AssertionsWebConfig.ResponseFormatterOptions;
    }

    [Fact]
    public void Serializer_IsAvailableByDefault()
    {
        AssertionsWebConfig.Serializer.Should().BeOfType<SystemTextJsonSerializer>();
    }

    [Fact]
    public void ResponseFormatterOptions_IsAvailableByDefault()
    {
        AssertionsWebConfig.ResponseFormatterOptions
            .Should().BeOfType<HttpResponseFormatterOptions>()
            .Which.MaximumReadableBytes.Should().Be(10 * 128 * 1024);
    }

    [Fact]
    public void Serializer_WhenSetToNull_ThrowsArgumentNullException()
    {
        Action act = () => AssertionsWebConfig.Serializer = null!;

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Serializer cannot be null.*");
    }

    [Fact]
    public void ResponseFormatterOptions_WhenSetToNull_ThrowsArgumentNullException()
    {
        Action act = () => AssertionsWebConfig.ResponseFormatterOptions = null!;

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("ResponseFormatterOptions cannot be null.*");
    }

    public void Dispose()
    {
        AssertionsWebConfig.Serializer = _initialSerializer;
        AssertionsWebConfig.ResponseFormatterOptions = _initialResponseFormatterOptions;
    }
}
