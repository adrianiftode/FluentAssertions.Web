using Microsoft.AspNetCore.Mvc.Testing;
using Sample.Api;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.IntegrationTests
{
    public class HttpResponseAssertionsIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HttpResponseAssertionsIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostABadRequest_AndAssertingIs200Ok_FailsWithExpectedErrorMessage()
        {
            // Arrange
            var client = _factory.CreateClient();
            var response = await client.PostAsync("/api/comments", new StringContent(@"{
                                          ""content"": ""Hey, you...""
                                        }", Encoding.UTF8, "application/json"));

            // Act
            Action act = () => response.Should().Be200Ok();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*The HTTP response was:*" +
                             "HTTP/1.1 400 BadRequest*"+
                             "The Author field is required.*" +
                             "\"content\": \"Hey, you...\"*");
        }
    }
}
