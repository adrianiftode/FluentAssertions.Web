using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public class FilesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public FilesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsOkResponse()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/files");

            // Assert
            response.Should().Be200Ok();
        }
    }
}
