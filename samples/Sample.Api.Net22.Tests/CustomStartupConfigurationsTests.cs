using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Api.Tests
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
                services.AddMvc();
            });
            builder.Configure(app => app
                .UseDeveloperExceptionPage()
                .Use((context, next) => {
                    throw new Exception("Wow!");
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
