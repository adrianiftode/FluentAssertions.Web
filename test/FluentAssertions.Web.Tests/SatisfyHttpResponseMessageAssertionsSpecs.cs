#if SH
namespace Shouldly.Web.Tests;
#elif AAV
namespace AwesomeAssertions.Web.Tests;
#else
namespace FluentAssertions.Web.Tests;
#endif

public class SatisfyHttpResponseMessageAssertionsSpecs
{
    [Fact]
    public void When_asserting_response_with_a_certain_assertion_to_satisfy_assertions_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(response => true.ShouldBeTrue());

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(response => true.Should().BeTrue());

        // Assert
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(c => c.Headers.AcceptRanges.ShouldContain("byte"), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject", "should satisfy all the conditions specified, but does not.", "Error 1", "c.Headers.AcceptRanges", "should contain", "\"byte\"", "but was actually", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(c => c.Headers.AcceptRanges.Should().Contain("byte"), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to satisfy one or more assertions, but it wasn't because we want to test the reason:*expected*{empty} to contain "byte"*HTTP response*""");
#endif
    }

    [Fact]
    public void When_asserting_response_without_having_satisfiable_assertion_to_satisfy_several_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

#if SH
        // Act: every condition is separate, so all failing ones are reported.
        Action act = () =>
            subject.ShouldSatisfy([
                r => r.Headers.AcceptRanges.ShouldContain("byte"),
                r => r.Headers.ShouldBeNull()], "because we want to test the reason");

        // Assert
        act.ShouldFailWith("subject", "should satisfy all the conditions specified, but does not.", "Error 1", "r.Headers.AcceptRanges", "should contain", "\"byte\"", "Error 2", "r.Headers", "should be null but was", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(
                response =>
                {
                    response.Headers.AcceptRanges.Should().Contain("byte");
                    response.Headers.Should().BeNull();
                }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to satisfy one or more assertions, but it wasn't because we want to test the reason:*expected*"byte"*expected*to be <null>*The HTTP response was:*""");
#endif
    }

    [Fact]
    public void When_asserting_response_to_satisfy_against_null_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy((Action<HttpResponseMessage>)null!);

        // Assert
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy((Action<HttpResponseMessage>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
#endif
    }

    [Fact]
    public void When_asserting_null_response_to_satisfy_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(response => true.ShouldBeTrue(), "because we want to test the failure message");

        // Assert
        act.ShouldFailWith("subject", "should not be null", "Additional Info:", "because we want to test the failure message");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(response => true.Should().BeTrue(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
}