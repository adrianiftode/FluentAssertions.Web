using FluentAssertions.Formatting;

namespace FluentAssertions.HttpMessageFormatter.Tests;

public class HttpResponseMessageFormatterTests
{
    [Fact]
    public void GivenUnspecifiedResponse_ShouldFormatWithFormattedObjectGraph()
    {
        // Arrange
        var formattedGraph = new FormattedObjectGraph(maxLines: 100);
        using var subject = new HttpResponseMessage();
        var sut = new HttpResponseMessageFormatter();

        // Act
        sut.Format(subject, formattedGraph, null!, null!);

        // Assert
        var formatted = formattedGraph.ToString();
        formatted.Should().Contain("The HTTP response was:");
        formatted.Should().Contain("HTTP/1.1 200 OK");
    }

    [Fact]
    public void GivenHttpResponseMessage_CanHandle_ReturnsTrue()
    {
        // Arrange
        using var response = new HttpResponseMessage();
        var sut = new HttpResponseMessageFormatter();

        // Act
        var result = sut.CanHandle(response);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GivenOtherObject_CanHandle_ReturnsFalse()
    {
        // Arrange
        var sut = new HttpResponseMessageFormatter();

        // Act
        var result = sut.CanHandle("not an http response");

        // Assert
        result.Should().BeFalse();
    }
}
