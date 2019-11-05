using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Api.Net30.Tests
{
    public class CustomStartupConfigurationsTests
    {
        [Fact]
        public async Task GetException_WhenDeveloperPageIsConfigured_ShouldBeInternalServerError()
        {
            // Arrange
            var builder = new WebHostBuilder();
            builder.ConfigureServices(services =>
            {
                services.AddRouting();
            });
            builder.Configure(app => app
                .UseDeveloperExceptionPage()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.Map("/exception", context =>
                    {
                        throw new Exception("Wow!", new Exception("Exactly!"));
                    });
                }));
            using var testServer = new TestServer(builder);
            using var client = testServer.CreateClient();

            // Act
            using var response = await client.GetAsync("/exception");

            // Assert
            response.Should().Be500InternalServerError();
        }
    }
}
