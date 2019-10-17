using FluentAssertions;
using FluentAssertions.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Api.Tests
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
            response.Should().Be200Ok();
        }

        [Fact]
        public async Task Get_WithId_ReturnsOkResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/values/1");

            // Assert
            response.Should().Be200Ok();
        }

        [Fact]
        public async Task Get_WithId_ReturnsOk_And_Expected_Model()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.GetAsync("/api/values/1");

            // Assert
            response.Should().Be200Ok().And.HaveContent<string>("value");
        }

        [Fact]
        public async Task Patch_ReturnsMethodNotAllowed()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PatchAsync("/api/values", new StringContent("", Encoding.UTF32, "application/json"));

            // Assert
            response.Should().Be405MethodNotAllowed();
        }

        [Fact]
        public async Task Post_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/values", new StringContent(@"""value""", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be200Ok();
        }

        [Fact]
        public async Task Post_WithNoContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/values", new StringContent("", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("A non-empty request body is required.");
        }
    }
}
