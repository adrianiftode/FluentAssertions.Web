namespace Assertions.Web.Tests;

public class SatisfyHttpResponseMessageAssertionsSpecs
{
    [Fact]
    public void When_asserting_response_with_a_certain_assertion_to_satisfy_assertions_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(response => true.ShouldBeTrue());
#else
        subject.Should().Satisfy(response => true.Should().BeTrue());
#endif
        };

        // Assert
#if SH
        act.ShouldNotThrow();
#else
    act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(c => c.Headers.AcceptRanges.ShouldContain("byte"), "we want to test the {0}", "reason");
#else
        subject.Should().Satisfy(c => c.Headers.AcceptRanges.Should().Contain("byte"), "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)Expected (.*) to satisfy one or more assertions, but it wasn't because we want to test the reason:(.*)expected(.*)\{empty\} to contain "byte"(.*)HTTP response(.*)""");
#else
    act.Should().Throw<XunitException>()
        .WithMessage("""Expected * to satisfy one or more assertions, but it wasn't because we want to test the reason:*expected*{empty} to contain "byte"*HTTP response*""");
#endif
    }

    [Fact]
    public void When_asserting_response_without_having_satisfiable_assertion_to_satisfy_several_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(
                response =>
                {
                    response.Headers.AcceptRanges.ShouldContain("byte");
                    response.Headers.ShouldBeNull();
                }, "we want to test the {0}", "reason");
#else
        subject.Should().Satisfy(
            response =>
            {
                response.Headers.AcceptRanges.Should().Contain("byte");
                response.Headers.Should().BeNull();
            }, "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)Expected (.*) to satisfy one or more assertions, but it wasn't because we want to test the reason:(.*)expected(.*)"byte"(.*)expected(.*)to be null(.*)The HTTP response was:(.*)""");
#else
    act.Should().Throw<XunitException>()
        .WithMessage("""Expected * to satisfy one or more assertions, but it wasn't because we want to test the reason:*expected*"byte"*expected*to be <null>*The HTTP response was:*""");
#endif
    }

    [Fact]
    public void When_asserting_response_to_satisfy_against_null_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy((Action<HttpResponseMessage>)null!);
#else
        subject.Should().Satisfy((Action<HttpResponseMessage>)null!);
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldMatch(@"(.*)Cannot verify the subject satisfies a `null` assertion\.(.*)");
#else
    act.Should().Throw<ArgumentNullException>()
        .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
#endif
    }

    [Fact]
    public void When_asserting_null_response_to_satisfy_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(response => true.ShouldBeTrue(), "because we want to test the failure {0}", "message");
#else
        subject.Should().Satisfy(response => true.Should().BeTrue(), "because we want to test the failure {0}", "message");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch(@"Expected a (.*) to assert because we want to test the failure message, but found <null>\.");
#else
    act.Should().Throw<XunitException>()
        .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
}