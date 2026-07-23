namespace HttpMessageFormatter.Tests;

public class HttpResponseFormatterExtensionsTests
{
    [Fact]
    public void GivenUnspecifiedResponse_ShouldFormatBasicResponse()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        var formatted = subject.Format();

        // Assert
        Assert.Contains("The HTTP response was:", formatted);
        Assert.Contains("HTTP/1.1 200 OK", formatted);
    }
}
