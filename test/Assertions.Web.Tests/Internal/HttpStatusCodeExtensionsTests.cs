using System.Net;

namespace Assertions.Web.Tests.Internal;

public class HttpStatusCodeExtensionsTests
{
    [Theory]
    [InlineData(100, true)]
    [InlineData(101, true)]
    [InlineData(199, true)]
    [InlineData(200, false)]
    [InlineData(301, false)]
    [InlineData(404, false)]
    [InlineData(500, false)]
    public void Given_a_status_code_Then_IsInformational_is_true_below_200(int code, bool expected)
        => ((HttpStatusCode)code).IsInformational().Should().Be(expected);

    [Theory]
    [InlineData(199, false)]
    [InlineData(200, true)]
    [InlineData(204, true)]
    [InlineData(299, true)]
    [InlineData(300, false)]
    [InlineData(301, false)]
    [InlineData(500, false)]
    public void Given_a_status_code_Then_IsSuccessful_is_true_for_200_to_299(int code, bool expected)
        => ((HttpStatusCode)code).IsSuccessful().Should().Be(expected);

    [Theory]
    [InlineData(299, false)]
    [InlineData(300, false)]
    [InlineData(301, true)]
    [InlineData(302, true)]
    [InlineData(308, true)]
    [InlineData(399, true)]
    [InlineData(400, false)]
    public void Given_a_status_code_Then_IsRedirection_is_true_for_301_to_399(int code, bool expected)
        => ((HttpStatusCode)code).IsRedirection().Should().Be(expected);

    [Theory]
    [InlineData(399, false)]
    [InlineData(400, true)]
    [InlineData(404, true)]
    [InlineData(499, true)]
    [InlineData(500, false)]
    [InlineData(200, false)]
    public void Given_a_status_code_Then_IsClientError_is_true_for_400_to_499(int code, bool expected)
        => ((HttpStatusCode)code).IsClientError().Should().Be(expected);

    [Theory]
    [InlineData(499, false)]
    [InlineData(500, true)]
    [InlineData(503, true)]
    [InlineData(599, true)]
    [InlineData(600, true)]
    [InlineData(404, false)]
    public void Given_a_status_code_Then_IsServerError_is_true_from_500(int code, bool expected)
        => ((HttpStatusCode)code).IsServerError().Should().Be(expected);
}
