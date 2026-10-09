namespace Assertions.Web.Tests.Internal;

public class ReadContentAsStringTests
{
    [Fact]
    public void Given_a_response_with_content_Then_it_reads_the_content_as_a_string()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("just some plain text")
        };

        var result = response.ReadContentAsString();

        result.Should().Be("just some plain text");
    }

    [Fact]
    public void Given_multi_line_unicode_content_Then_it_is_read_verbatim()
    {
        const string payload = "first line\r\nsecond line: héllo wörld ✔";
        using var response = new HttpResponseMessage
        {
            Content = new StringContent(payload)
        };

        var result = response.ReadContentAsString();

        result.Should().Be(payload);
    }

    [Fact]
    public void Given_an_empty_content_Then_it_reads_an_empty_string()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent(string.Empty)
        };

        var result = response.ReadContentAsString();

        result.Should().BeEmpty();
    }
}
