using System.Net.Http;
using AwesomeAssertions.Formatting;
using HttpMessageFormatter;

namespace AwesomeAssertions.Web.AwesomeAssertionsWebConfig.Tests;

[Collection("Response Formatter Options Tests")]
public sealed class ResponseFormatterOptionsTests : IDisposable
{
    private readonly HttpResponseFormatterOptions _initialResponseFormatterOptions;

    public ResponseFormatterOptionsTests()
    {
        _initialResponseFormatterOptions = AwesomeAssertions.AwesomeAssertionsWebConfig.ResponseFormatterOptions;
    }

    [Fact]
    public void ResponseFormatterOptions_IsAvailableByDefault()
    {
        AwesomeAssertions.AwesomeAssertionsWebConfig.ResponseFormatterOptions
            .Should().BeOfType<HttpResponseFormatterOptions>()
            .Which.MaximumReadableBytes.Should().Be(10 * 128 * 1024);
    }

    [Fact]
    public void ResponseFormatterOptions_ShouldLimitTheFormattedResponseContent()
    {
        // Arrange
        AwesomeAssertions.AwesomeAssertionsWebConfig.ResponseFormatterOptions = new HttpResponseFormatterOptions
        {
            MaximumReadableBytes = 20
        };
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(new string('-', 50) + new string('+', 50))
        };
        var formattedGraph = new FormattedObjectGraph(maxLines: 100);

        // Act
        new HttpResponseMessageFormatter().Format(subject, formattedGraph, null!, null!);

        // Assert
        var formatted = formattedGraph.ToString();
        formatted.Should().Match("*Content is too large to display*")
            .And.Contain(new string('-', 20))
            .And.NotContain("+");
    }

    public void Dispose()
    {
        AwesomeAssertions.AwesomeAssertionsWebConfig.ResponseFormatterOptions = _initialResponseFormatterOptions;
    }
}
