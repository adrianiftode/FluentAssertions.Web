using FluentAssertions.Web.Internal.ContentProcessors;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal.ContentProcessors
{
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
    }
}
