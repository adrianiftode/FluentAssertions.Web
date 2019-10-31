using FluentAssertions.Web.Internal.ContentProcessors;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal.ContentProcessors
{
    public class StreamProcessorTests
    {
        [Fact]
        public async Task GivenContentTypeOtherThanStream_WhenGetContentInfo_ThenIsEmpty()
        {
            // Arrange
            var content = new StringContent("", Encoding.UTF8, "application/xml");
            var sut = new StreamProcessor(content);
            var contentBuilder = new StringBuilder();

            // Act
            await sut.GetContentInfo(contentBuilder);

            // Assert
            contentBuilder.ToString().Should().BeEmpty();
        }

        [Fact]
        public async Task GivenAStreamContent_WhenGetContentInfo_ThenIsEmptyAsEverythingIsInTheHeaders()
        {
            // Arrange
            var content = new StreamContent(new MemoryStream(new byte[] { 0 }));
            content.Headers.Add("Content-Type", "application/octet-stream");

            var sut = new StreamProcessor(content);
            var contentBuilder = new StringBuilder();

            // Act
            await sut.GetContentInfo(contentBuilder);

            // Assert
            contentBuilder.ToString().Should().BeEmpty();
        }
    }
}