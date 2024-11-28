using FluentAssertions;
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

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(1000)] // this is not truncated
        [InlineData(2000)] // this is not truncated
        public async Task GetData_ReturnsOkResponse(int howMuchData)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/values/generated/{howMuchData}");

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

#if NETCOREAPP2_1 || NETCOREAPP2_2
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
        public async Task Post_ReturnsEmptyContent()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/values", new StringContent(@"""value""", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().BeEmpty();
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
#if NETCOREAPP2_2 || NET5_0_OR_GREATER
                        response.Should().Be400BadRequest()
                                .And.HaveErrorMessage("A non-empty request body is required.");
#elif NETCOREAPP3_0 || NETCOREAPP3_1
                        response.Should().Be400BadRequest()
                                .And.HaveErrorMessage("*The input does not contain any JSON tokens*");
#endif
        }

        [Fact]
        public async Task Get_Should_Contain_a_Header_With_Correlation_Id_That_Matches_A_Guid_Like_Pattern()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/values");

            // Assert
            response.Should().HaveHeader("X-Correlation-ID").And.Match("*-*", "we want to test the correlation id is a Guid like one");
        }

        [Fact]
        public async Task Get_Should_Contain_a_Header_With_Correlation_Id()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/values");

            // Assert
            response.Should().HaveHeader("X-Correlation-ID").And.NotBeEmpty("we want to test the correlation id header has some value");
        }
    }
}
