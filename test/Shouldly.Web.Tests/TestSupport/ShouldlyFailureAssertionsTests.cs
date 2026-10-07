namespace Shouldly.Web.Tests.TestSupport;

/// <summary>
/// Tests for <see cref="ShouldlyFailureAssertions"/> itself: the helper is the only thing standing
/// between a broken spec and a green run, so its five failure modes are pinned here.
/// </summary>
public class ShouldlyFailureAssertionsTests
{
    private static readonly Action ThrowsWithLf = () =>
        throw new ShouldAssertException("subject.StatusCode\n    should be\nHttpStatusCode.Created");

    private static readonly Action ThrowsWithCrlf = () =>
        throw new ShouldAssertException("subject.StatusCode\r\n    should be\r\nHttpStatusCode.Created");

    [Fact]
    public void A_failing_action_with_the_right_first_line_and_fragments_passes()
    {
        var message = ThrowsWithLf.ShouldFailWith(
            "subject.StatusCode", "should be", "HttpStatusCode.Created");

        message.ShouldBe("subject.StatusCode\n    should be\nHttpStatusCode.Created");
    }

    [Fact]
    public void A_wrong_first_line_fails()
    {
        Should.Throw<ShouldAssertException>(() => ThrowsWithLf.ShouldFailWith(
                "response.StatusCode", "should be"))
            .Message.ShouldContain("response.StatusCode");
    }

    [Fact]
    public void A_missing_fragment_fails()
    {
        Should.Throw<ShouldAssertException>(() => ThrowsWithLf.ShouldFailWith(
                "subject.StatusCode", "but was"))
            .Message.ShouldContain("but was");
    }

    [Fact]
    public void Fragments_that_are_out_of_order_fail()
    {
        Should.Throw<ShouldAssertException>(() => ThrowsWithLf.ShouldFailWith(
                "subject.StatusCode", "HttpStatusCode.Created", "should be"))
            .Message.ShouldContain("after position");
    }

    [Fact]
    public void Crlf_in_the_message_matches_an_lf_fragment()
    {
        var message = ThrowsWithCrlf.ShouldFailWith(
            "subject.StatusCode", "should be\nHttpStatusCode.Created");

        message.ShouldStartWith("subject.StatusCode\n");
    }
}
