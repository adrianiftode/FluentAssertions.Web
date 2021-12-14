using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
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
    public class CustomStartupConfigurationsTests
    {
        [Fact]
        public async Task GetException_WhenDeveloperPageIsConfigured_ShouldBeInternalServerError()
        {
            // Arrange
            var builder = new WebHostBuilder();
            builder.ConfigureServices(services =>
            {
#if NETCOREAPP2_2
                services.AddMvc();
#else
                services.AddRouting();
#endif
            });
            builder.Configure(app => app
                .UseDeveloperExceptionPage()
#if NETCOREAPP2_2

                .Use((context, next) => throw new Exception("Wow!", new Exception("Exactly!"))))
#endif
#if NETCOREAPP3_0 || NETCOREAPP3_1
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.Map("/exception", context =>
                    {
                        throw new Exception("Wow!", new Exception("Exactly!"));
                    });
                }))
#endif
                ;
            using var testServer = new TestServer(builder);
            using var client = testServer.CreateClient();

            // Act
            using var response = await client.GetAsync("/exception");

            // Assert
            response.Should().Be500InternalServerError();
        }
    }
}
