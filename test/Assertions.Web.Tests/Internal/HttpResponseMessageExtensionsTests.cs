namespace Assertions.Web.Tests.Internal;

public class HttpResponseMessageExtensionsTests
{
    [Fact]
    public void Given_a_request_with_headers_and_content_Then_GetHeaders_unions_both()
    {
        using var request = new HttpRequestMessage
        {
            Content = new StringContent("payload")
        };
        request.Headers.Add("X-Request", "one");

        var headers = request.GetHeaders().ToArray();

        headers.Select(header => header.Key).Should().Contain("X-Request").And.Contain("Content-Type");
    }

    [Fact]
    public void Given_a_request_without_content_Then_GetHeaders_returns_only_the_request_headers()
    {
        using var request = new HttpRequestMessage();
        request.Headers.Add("X-Request", "one");

        var headers = request.GetHeaders().ToArray();

        headers.Should().ContainSingle()
            .Which.Key.Should().Be("X-Request");
    }
}
