using Shouldly.Web.Tests.TestModels;
using Shouldly.Web.Tests.TestSupport;

namespace Shouldly.Web.Tests.Internal;

/// <summary>
/// SH-only tests for the multi-conditions overloads of <c>ShouldSatisfy</c>. Every assertion passed as
/// its own condition runs inside Shouldly's satisfy-all-conditions mechanism, so all failing conditions
/// are reported — unlike a single assertion whose lambda body holds several statements, which stops at
/// its first failure. The tests also pin the message layout: one error block per failing condition, the
/// HTTP response dump appended once, and no duplicated message coming from an inner exception.
/// </summary>
public class SatisfyMultipleConditionsTests
{
    [Fact]
    public void All_failing_response_conditions_are_reported()
    {
        using var response = new HttpResponseMessage();

        ((Action)(() => response.ShouldSatisfy([
            r => r.Headers.AcceptRanges.ShouldContain("byte"),
            r => r.Headers.ShouldBeNull()], "we need it")))
            .ShouldFailWith(
                "response",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "r.Headers.AcceptRanges",
                "should contain",
                "\"byte\"",
                "Error 2",
                "r.Headers",
                "should be null but was",
                "Additional Info:",
                "we need it",
                "The HTTP response was:");
    }

    [Fact]
    public void All_failing_model_conditions_are_reported()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("""{ "property": "Value" }""")
        };

        ((Action)(() => response.ShouldSatisfy<TestModel>([
            m => m.Property.ShouldBe("Not Value"),
            m => m.ShouldBeNull()])))
            .ShouldFailWith(
                "response.Content",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "m.Property",
                "should be",
                "\"Not Value\"",
                "but was",
                "\"Value\"",
                "Error 2",
                "should be null but was",
                "The HTTP response was:");
    }

    [Fact]
    public void All_failing_conditions_inferred_from_a_model_structure_are_reported()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("""{ "property": "Value" }""")
        };

        ((Action)(() => response.ShouldSatisfy(new
        {
            Property = default(string)
        }, [
            m => m.Property.ShouldBe("Not Value"),
            m => m.ShouldBeNull()], "we need it")))
            .ShouldFailWith(
                "response.Content",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "m.Property",
                "should be",
                "\"Not Value\"",
                "but was",
                "\"Value\"",
                "Error 2",
                "should be null but was",
                "Additional Info:",
                "we need it",
                "The HTTP response was:");
    }

    [Fact]
    public void Only_the_failing_conditions_are_reported()
    {
        using var response = new HttpResponseMessage();

        var message = ((Action)(() => response.ShouldSatisfy([
            r => r.Headers.ShouldNotBeNull(),
            r => r.Headers.AcceptRanges.ShouldContain("byte")])))
            .ShouldFailWith(
                "response",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "r.Headers.AcceptRanges",
                "should contain",
                "\"byte\"",
                "The HTTP response was:");

        message.ShouldNotContain("Error 2");
    }

    [Fact]
    public void An_enumerable_variable_of_conditions_is_supported()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("""{ "property": "Value" }""")
        };
        IEnumerable<Action<TestModel>> conditions =
        [
            m => m.Property.ShouldBe("Not Value"),
            m => m.Property.ShouldBe("Value")
        ];

        var message = ((Action)(() => response.ShouldSatisfy(conditions)))
            .ShouldFailWith(
                "response.Content",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "m.Property",
                "should be",
                "\"Not Value\"",
                "but was",
                "\"Value\"",
                "The HTTP response was:");

        message.ShouldNotContain("Error 2");
    }

    [Fact]
    public void An_array_variable_of_conditions_is_supported()
    {
        using var response = new HttpResponseMessage();
        Action<HttpResponseMessage>[] conditions =
        [
            r => r.Headers.AcceptRanges.ShouldContain("byte"),
            r => r.Headers.ShouldBeNull()
        ];

        ((Action)(() => response.ShouldSatisfy(conditions, "we need it")))
            .ShouldFailWith(
                "response",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "r.Headers.AcceptRanges",
                "should contain",
                "\"byte\"",
                "Error 2",
                "r.Headers",
                "should be null but was",
                "Additional Info:",
                "we need it",
                "The HTTP response was:");
    }

    [Fact]
    public void The_failure_message_is_reported_exactly_once()
    {
        using var response = new HttpResponseMessage();

        var message = ((Action)(() => response.ShouldSatisfy([
            r => r.Headers.AcceptRanges.ShouldContain("byte"),
            r => r.Headers.ShouldBeNull()])))
            .ShouldFailWith(
                "response",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "Error 2",
                "The HTTP response was:");

        CountOccurrences(message, "should satisfy all the conditions specified, but does not.").ShouldBe(1);
        CountOccurrences(message, "---------------- Error 1 ----------------").ShouldBe(1);
        CountOccurrences(message, "The HTTP response was:").ShouldBe(1);
    }

    [Fact]
    public void Nested_shouldly_web_conditions_are_condensed_into_a_single_dump()
    {
        using var message = new HttpResponseMessage(HttpStatusCode.OK);

        var failure = ((Action)(() => message.ShouldSatisfy([
            r => r.ShouldBe201Created(),
            r => r.ShouldHaveHeader("X-A")])))
            .ShouldFailWith(
                "message",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "Error 2",
                "The HTTP response was:");

        // Both conditions fail through Shouldly.Web, so before the suppression each would append its own
        // dump and the outer assertion a third one. Exactly one dump must survive.
        CountOccurrences(failure, "The HTTP response was:").ShouldBe(1);
    }

    [Fact]
    public void The_thrown_exception_has_no_inner_exception_printing_the_same_failure_again()
    {
        using var response = new HttpResponseMessage();

        var exception = Record.Exception(() => response.ShouldSatisfy([
            r => r.Headers.AcceptRanges.ShouldContain("byte")]));

        var shouldAssert = exception.ShouldBeOfType<ShouldAssertException>();
        shouldAssert.InnerException.ShouldBeNull();
    }

    [Fact]
    public void Null_condition_arguments_are_rejected()
    {
        using var response = new HttpResponseMessage();

        ((Action)(() => response.ShouldSatisfy((Action<HttpResponseMessage>[])null!)))
            .ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");

        ((Action)(() => response.ShouldSatisfy((IEnumerable<Action<HttpResponseMessage>>)null!)))
            .ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");

        ((Action)(() => response.ShouldSatisfy<TestModel>((IEnumerable<Action<TestModel>>)null!)))
            .ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");

        ((Action)(() => response.ShouldSatisfy(new TestModel(), (IEnumerable<Action<TestModel>>)null!)))
            .ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");
    }

    private static int CountOccurrences(string text, string fragment)
    {
        var count = 0;
        for (var index = text.IndexOf(fragment, StringComparison.Ordinal); index >= 0;
             index = text.IndexOf(fragment, index + fragment.Length, StringComparison.Ordinal))
        {
            count++;
        }

        return count;
    }
}
