using FluentAssertions.Web.Internal.ContentProcessors;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal.ContentProcessors
{
    public class MultipartProcessorTests
    {
        [Fact]
        public async Task GivenContentTypeOtherThanMultipart_WhenGetContentInfo_ThenIsEmpty()
        {
            // Arrange
            var content = new StringContent("", Encoding.UTF8, "application/xml");
            var sut = new MultipartProcessor(content);
            var contentBuilder = new StringBuilder();

            // Act
            await sut.GetContentInfo(contentBuilder);

            // Assert
            contentBuilder.ToString().Should().BeEmpty();
        }

        [Fact]
        public async Task GivenAMultipartContent_WhenGetContentInfo_ThenIsEmptyAsEverythingIsInTheHeaders()
        {
            // Arrange
            var content = new MultipartContent
            {
                new ReadOnlyMemoryContent(new ReadOnlyMemory<byte>(new byte[] {0})),
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"name", "John"}, {"location", "Alabama"},
                })
            };

            var sut = new MultipartProcessor(content);
            var contentBuilder = new StringBuilder();

            // Act
            await sut.GetContentInfo(contentBuilder);

            // Assert
            contentBuilder.ToString().Should().BeEmpty();
        }
    }
}