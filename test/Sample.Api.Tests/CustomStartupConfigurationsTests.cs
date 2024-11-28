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
#if NETCOREAPP2_1 || NETCOREAPP2_2
                services.AddMvc();
#else
                services.AddRouting();
#endif
            });
            builder.Configure(app => app
                    .UseDeveloperExceptionPage(
#if NET5_0_OR_GREATER
                    new DeveloperExceptionPageOptions()
#endif
                    )
#if NETCOREAPP2_1 || NETCOREAPP2_2

                .Use((context, next) => throw new Exception("Wow!", new Exception("Exactly!")))
#endif
#if NETCOREAPP3_0_OR_GREATER
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.Map("/exception", context => 
                        throw new Exception("Wow!", new Exception("Exactly!")));
                })
#endif
                );
            using var testServer = new TestServer(builder);
            using var client = testServer.CreateClient();

            // Act
            using var response = await client.GetAsync("/exception");

            // Assert
            response.Should().Be500InternalServerError();
        }
    }
}
