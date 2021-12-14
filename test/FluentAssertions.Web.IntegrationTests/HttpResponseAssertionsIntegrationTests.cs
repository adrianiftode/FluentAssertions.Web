using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
#if NETCOREAPP2_2
using Sample.Api.Net22;
#endif
#if NETCOREAPP3_0
using Sample.Api.Net30;
#endif
#if NETCOREAPP3_1
using Sample.Api.Net31;
#endif
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
