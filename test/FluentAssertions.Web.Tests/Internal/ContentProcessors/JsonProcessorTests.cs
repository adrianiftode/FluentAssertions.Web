using FluentAssertions.Web.Internal.ContentProcessors;

namespace FluentAssertions.Web.Tests.Internal.ContentProcessors;

public class JsonProcessorTests
{
    [Fact]
    public async Task GivenHttpResponseWithNoContent_WhenGetContentInfo_ThenIsEmpty()
    {
        // Arrange
        using var response = new HttpResponseMessage();
        var sut = new JsonProcessor(response.Content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task GivenContentTypeOtherThanApplicationJson_WhenGetContentInfo_ThenIsEmpty()
    {
        // Arrange
        var content = new StringContent("", Encoding.UTF8, "application/xml");
        var sut = new JsonProcessor(content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task GivenANonJsonContentWithAnApplicationJsonMediaType_WhenGetContentInfo_ThenItIsTheExpectedJson()
    {
        // Arrange
        var content = new StringContent(@"giberish", Encoding.UTF8, "application/json");
        var sut = new JsonProcessor(content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().Match(@"giberish");
    }

    [Fact]
    public async Task GivenAJsonContentWithAnApplicationJsonMediaType_WhenGetContentInfo_ThenItIsTheExpectedJson()
    {
        // Arrange
        var content = new StringContent(@"{ ""a"": ""json""}", Encoding.UTF8, "application/json");
        var sut = new JsonProcessor(content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().Match(@"{*
*""a"": ""json""*
}*");
    }

    [Fact]
    public async Task GivenHttpResponseWithDisposedContent_WhenGetContentInfo_ThenIsEmpty()
    {
        // Arrange
        var content = new StringContent(@"giberish", Encoding.UTF8, "application/json");
        var sut = new JsonProcessor(content);
        var contentBuilder = new StringBuilder();
        content.Dispose();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().BeEmpty();
    }
}
