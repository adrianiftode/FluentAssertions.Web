using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
#if NETCOREAPP2_2
using Sample.Api.Net22;
using Sample.Api.Net22.Controllers;
#endif
#if NETCOREAPP3_0
using Sample.Api.Net30;
using Sample.Api.Net30.Controllers;
#endif
#if NETCOREAPP3_1
using Sample.Api.Net31;
using Sample.Api.Net31.Controllers;
#endif

namespace Sample.Api.Net31.Tests
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
            response.Should().Be200Ok().And.BeAs<string>("value");
        }

#if NETCOREAPP2_2
        [Fact]
        public async Task Patch_Returns404NotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PatchAsync("/api/values", new StringContent("", Encoding.UTF32, "application/json"));

            // Assert
            response.Should().Be404NotFound("It's .Net Core 2.2");
        }
#endif

#if NETCOREAPP3_0 || NETCOREAPP3_1
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
#endif

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
#if NETCOREAPP2_2
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("A non-empty request body is required.");
#elif NETCOREAPP3_0 || NETCOREAPP3_1
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("*The input does not contain any JSON tokens*");
#endif
        }
    }
}
