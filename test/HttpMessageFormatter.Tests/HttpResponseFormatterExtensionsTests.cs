using System;

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

    [Fact]
    public void GivenOptionsWithSmallerMaximumReadableBytes_WhenFormat_ThenOnlyAPartOfTheContentIsPrinted()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(new string('-', 50) + new string('+', 50))
        };
        var options = new HttpResponseFormatterOptions
        {
            MaximumReadableBytes = 20
        };

        // Act
        var formatted = subject.Format(options);

        // Assert
        Assert.Contains("Content is too large to display", formatted);
        Assert.Contains(new string('-', 20), formatted);
        Assert.DoesNotContain("+", formatted);
    }

    [Fact]
    public void GivenDefaultOptions_WhenFormat_ThenTheWholeContentIsPrinted()
    {
        // Arrange
        var content = string.Concat(Enumerable.Repeat("-abc", 1000));
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(content)
        };
        var options = new HttpResponseFormatterOptions();

        // Act
        var formatted = subject.Format(options);

        // Assert
        Assert.DoesNotContain("too large", formatted);
        Assert.Contains(content, formatted);
    }

    [Fact]
    public void GivenMaximumReadableBytesNotGreaterThanZero_WhenSet_ThenThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = new HttpResponseFormatterOptions();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => options.MaximumReadableBytes = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => options.MaximumReadableBytes = -1);
    }
}
