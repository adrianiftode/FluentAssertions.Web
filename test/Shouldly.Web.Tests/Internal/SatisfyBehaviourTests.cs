using System.Net;
using Shouldly.Web.Tests.TestModels;
using Shouldly.Web.Tests.TestSupport;

namespace Shouldly.Web.Tests.Internal;

/// <summary>
/// SH-only behaviour tests for ShouldSatisfy. Shouldly's native ShouldSatisfy reports every
/// exception thrown inside the assertion lambda as a failure: a non-assertion exception (any
/// throw) becomes a failure whose text contains the exception message, and never escapes the
/// assertion. The capture helpers assert both properties: the action must throw a
/// <see cref="ShouldAssertException"/> (not the original exception), and its message must
/// contain the exception text inside the "Error 1" slot.
/// </summary>
public class SatisfyBehaviourTests
{
    [Fact]
    public void A_non_assertion_exception_in_a_response_lambda_is_reported_as_a_failure()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{ "value": "Hey" }""")
        };

        ((Action)(() => response.ShouldSatisfy(_ => throw new InvalidOperationException("boom!"), "we need it")))
            .ShouldFailWith(
                "response",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "boom!",
                "Additional Info:",
                "we need it",
                "The HTTP response was:");
    }

    [Fact]
    public void A_non_assertion_exception_in_a_model_lambda_is_reported_as_a_failure()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""{ "property": "Value", "other": null }""")
        };

        ((Action)(() => response.ShouldSatisfy<TestModel>(_ => throw new InvalidOperationException("boom!"), "we need it")))
            .ShouldFailWith(
                "response.Content",
                "should satisfy all the conditions specified, but does not.",
                "Error 1",
                "boom!",
                "Additional Info:",
                "we need it",
                "The HTTP response was:");
    }
}