﻿namespace FluentAssertions.Web;

public partial class HttpResponseMessageAssertions
{
    /// <summary>
    /// Asserts that an HTTP response satisfies an assertion.
    /// </summary>
    /// <param name="assertion">
    /// An assertion about the HTTP response.
    /// </param>
    /// <remarks>
    /// The assertion can be a single assertion or a collection of assertions if the assertion action is expressed as a statement lambda.
    /// </remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public AndConstraint<HttpResponseMessageAssertions> Satisfy(Action<HttpResponseMessage> assertion,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");

        CurrentAssertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");

        var failuresFromAssertions = CollectFailuresFromAssertion(assertion!, Subject);

        if (failuresFromAssertions.Any())
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:response} to satisfy one or more assertions, but it wasn't{reason}: {0}{1}",
                    new AssertionsFailures(failuresFromAssertions), Subject);
        }

        return new AndConstraint<HttpResponseMessageAssertions>(this);
    }

    /// <summary>
    /// Asserts that an HTTP response satisfies an asynchronous assertion.
    /// </summary>
    /// <param name="assertion">
    /// An assertion about the HTTP response.
    /// </param>
    /// <remarks>
    /// The assertion can be a single assertion or a collection of assertions if the assertion action is expressed as a statement lambda.
    /// </remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public AndConstraint<HttpResponseMessageAssertions> Satisfy(Func<HttpResponseMessage, Task> assertion,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");

        CurrentAssertionChain
            .ForCondition(Subject is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");

        var failuresFromAssertions = CollectFailuresFromAssertion(asserted =>
        {
            Func<Task> assertionExecutor = () => assertion(asserted!);
            assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }, Subject);

        if (failuresFromAssertions.Any())
        {
            CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:response} to satisfy one or more assertions, but it wasn't{reason}: {0}{1}",
                    new AssertionsFailures(failuresFromAssertions), Subject);
        }

        return new AndConstraint<HttpResponseMessageAssertions>(this);
    }
}
