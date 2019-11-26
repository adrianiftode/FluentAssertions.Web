using FluentAssertions;
using Flurl;
using Flurl.Http;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Api.Net30.Tests
{
    public class ShareTests
    {
        static ShareTests()
        {
            FlurlHttp.Configure(settings =>
            {
                settings.AllowedHttpStatusRange = "*";
                settings.Timeout = TimeSpan.FromMinutes(2);
            });
        }
        [Fact]
        public async Task Post_WhenNoAuthorizationInfo_ReturnsUnauthorized()
        {
            //Act
            var response = await "https://ar.isharetest.net/"
                .AppendPathSegment("delegation")
                .PostAsync(new StringContent("", Encoding.UTF8, "application/json"));

            //Assert
            response.Should().Be401Unauthorized();
        }
    }
}
