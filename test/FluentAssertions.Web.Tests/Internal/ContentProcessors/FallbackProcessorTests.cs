using FluentAssertions.Web.Internal.ContentProcessors;
using System.Net.Http.Headers;

namespace FluentAssertions.Web.Tests.Internal.ContentProcessors;

public class FallbackProcessorTests
{
    [Fact]
    public async Task GivenHttpResponseWithNoContent_WhenGetContentInfo_ThenIsEmpty()
    {
        // Arrange
        using var response = new HttpResponseMessage();
        var sut = new FallbackProcessor(response.Content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task GivenHttpResponseWithContent_WhenGetContentInfo_ThenItContainsTheContent()
    {
        // Arrange
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("the content")
        };
        var sut = new FallbackProcessor(response.Content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().Be("the content");
    }

    [Fact]
    public async Task GivenContentWithLengthTooLarge_WhenGetContentInfo_ThenItContainsAWarningMessage()
    {
        // Arrange
        using var response = new HttpResponseMessage
        {
            Content = new StringContent(new string(Enumerable
                .Range(0, ContentFormatterOptions.MaximumReadableBytes + 1)
                .Select(_ => '0')
                .ToArray()))
        };
        var sut = new FallbackProcessor(response.Content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().Match("*too large*");
    }

    [Fact]
    public async Task GivenHttpResponseWithDisposedContent_WhenGetContentInfo_ThenIsEmpty()
    {
        // Arrange
        var content = new StringContent("the content");
        using var response = new HttpResponseMessage
        {
            Content = content
        };
        var sut = new FallbackProcessor(response.Content);
        var contentBuilder = new StringBuilder();
        content.Dispose();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().Match(
            "*Content is disposed so it cannot be read.*");
    }

    [Fact]
    public async Task GivenStreamResponse_WhenGetContentInfo_ThenTryToPrint()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(new MemoryStream(new byte[1]))
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("image/jpeg")
                }
            }
        };
        var sut = new FallbackProcessor(subject.Content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GivenByteArrayResponse_WhenGetContentInfo_ThenTryToPrint()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(new byte[1])
        };
        var sut = new FallbackProcessor(subject.Content);
        var contentBuilder = new StringBuilder();

        // Act
        await sut.GetContentInfo(contentBuilder);

        // Assert
        contentBuilder.ToString().Should().NotBeNullOrEmpty();
    }
}
