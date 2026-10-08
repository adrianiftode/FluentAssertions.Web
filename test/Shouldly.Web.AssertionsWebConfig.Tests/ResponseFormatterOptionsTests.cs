using System.Net;
using System.Net.Http;
using HttpMessageFormatter;
using Shouldly;

namespace Shouldly.Web.Tests;

[Collection("Response Formatter Options Tests")]
public sealed class ResponseFormatterOptionsTests : IDisposable
{
    private readonly HttpResponseFormatterOptions _initialResponseFormatterOptions;

    public ResponseFormatterOptionsTests()
    {
        _initialResponseFormatterOptions = AssertionsWebConfig.ResponseFormatterOptions;
    }

    [Fact]
    public void ResponseFormatterOptions_IsAvailableByDefault()
    {
        AssertionsWebConfig.ResponseFormatterOptions
            .ShouldBeOfType<HttpResponseFormatterOptions>()
            .MaximumReadableBytes.ShouldBe(10 * 128 * 1024);
    }

    [Fact]
    public void ResponseFormatterOptions_ShouldLimitTheFormattedResponseContent()
    {
        // Arrange
        AssertionsWebConfig.ResponseFormatterOptions = new HttpResponseFormatterOptions
        {
            MaximumReadableBytes = 20
        };
        using var subject = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(new string('-', 50) + new string('+', 50))
        };

        // Act
        var act = () => subject.ShouldBe200Ok();

        // Assert
        var message = act.ShouldThrow<ShouldAssertException>().Message;
        message.ShouldContain("Content is too large to display");
        message.ShouldContain(new string('-', 20));
        message.ShouldNotContain("+");
    }

    public void Dispose()
    {
        AssertionsWebConfig.ResponseFormatterOptions = _initialResponseFormatterOptions;
    }
}