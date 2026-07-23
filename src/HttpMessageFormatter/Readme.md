 **HttpMessageFormatter** is a utility library for formatting HTTP request and response messages for inspection and debugging.
      It provides rich, readable output of HTTP messages including headers, content, status codes, and more.
      This library has no dependencies on any assertion framework, making it suitable for general-purpose use.

### Basic Usage

```csharp
using HttpMessageFormatter;

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

```