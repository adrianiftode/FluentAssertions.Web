using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Web.Api.Tests
{
    public class ValuesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ValuesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsOkResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/values");

            // Assert
            await response.Should().BeOk();
        }

        [Fact]
        public async Task Patch_ReturnsMethodNotAllowed()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PatchAsync("/api/values", new StringContent(""));

            // Assert
            await response.Should().BeMethodNotAllowed();
        }
    }
}
