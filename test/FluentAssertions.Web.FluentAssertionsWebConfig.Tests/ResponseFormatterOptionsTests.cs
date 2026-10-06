using System.Net.Http;
using FluentAssertions.Formatting;
using HttpMessageFormatter;

namespace FluentAssertions.Web.FluentAssertionsWebConfig.Tests;

[Collection("Response Formatter Options Tests")]
public sealed class ResponseFormatterOptionsTests : IDisposable
{
    private readonly HttpResponseFormatterOptions _initialResponseFormatterOptions;

    public ResponseFormatterOptionsTests()
    {
        _initialResponseFormatterOptions = FluentAssertions.FluentAssertionsWebConfig.ResponseFormatterOptions;
    }

    [Fact]
    public void ResponseFormatterOptions_IsAvailableByDefault()
    {
        FluentAssertions.FluentAssertionsWebConfig.ResponseFormatterOptions
            .Should().BeOfType<HttpResponseFormatterOptions>()
            .Which.MaximumReadableBytes.Should().Be(10 * 128 * 1024);
    }

    [Fact]
    public void ResponseFormatterOptions_ShouldLimitTheFormattedResponseContent()
    {
        // Arrange
        FluentAssertions.FluentAssertionsWebConfig.ResponseFormatterOptions = new HttpResponseFormatterOptions
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
        FluentAssertions.FluentAssertionsWebConfig.ResponseFormatterOptions = _initialResponseFormatterOptions;
    }
}
