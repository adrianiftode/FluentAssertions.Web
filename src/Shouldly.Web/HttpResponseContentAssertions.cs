// ReSharper disable CheckNamespace

namespace Shouldly;

/// <summary>
/// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state related to the HTTP content.
/// </summary>
[ShouldlyMethods]
public static class HttpResponseContentAssertions
{
    /// <summary>
    /// Asserts that the HTTP content is empty.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBeEmpty(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        var content = actual!.GetContent();

        ExecuteAssertion
            .ForCondition(string.IsNullOrEmpty(content))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have no content. {0}", new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that HTTP actual content can be an equivalent representation of the expected model.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="expectedModel">
    /// The expected model.
    /// </param>
    /// <param name="responseExpression"></param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBeAs<TModel>(this HttpResponseMessage? actual, TModel expectedModel, [CallerArgumentExpression(nameof(actual))] string responseExpression = "actual", string because = "", params object[] becauseArgs)
        => actual.ShouldBeAs(expectedModel, null, responseExpression, because, becauseArgs);

    /// <summary>
    /// Asserts that HTTP actual content can be an equivalent representation of the expected model.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="expectedModel">
    /// The expected model.
    /// </param>
    /// <param name="options">
    //    /// A reference to the <see cref="EquivalencyOptions"/> configuration object that can be used
    //    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    //    /// <see cref="EquivalencyOptions"/> class. The global defaults are determined by the
    //    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBeAs<TModel>(this HttpResponseMessage? actual, TModel expectedModel, EquivalencyOptions? options, [CallerArgumentExpression("actual")] string responseExpression = "actual", string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, responseExpression)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        if (expectedModel == null)
        {
            throw new ArgumentNullException(nameof(expectedModel), "Cannot verify having a content equivalent to a model against a <null> model.");
        }

        var expectedModelType = expectedModel.GetType();

        var (success, errorMessage) = actual!.TryGetSubjectModel(out var subjectModel, expectedModelType);

        ExecuteAssertion
            .ForCondition(success, responseExpression)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                expectedModelType.ToString() ?? "unknown type", new FormatHttpResponseMessage(actual), errorMessage);

        AssertionsFailure? failure = null;

        try
        {
            subjectModel.ShouldSatisfy(
                [s => s.ShouldBeEquivalentTo(expectedModel, options ?? new EquivalencyOptions())]);
        }
        catch (ShouldAssertException ex)
        {
            failure = ex.ExtractAssertionFailure();
        }

        ExecuteAssertion
                   .ForCondition(failure == null, responseExpression)
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:actual} to have a content equivalent to a model{reason}, but it has differences:{0} {1}",
                       new FormatAssertionsFailure(failure),
                       new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that HTTP actual has content that matches a wildcard pattern.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="expectedWildcardText">
    /// The wildcard pattern with which actual is matched, where * and ? have special meanings.
    /// <remarks>
    ///     <para>* - Matches any number of characters. You can use the asterisk (*) anywhere in a character string. Example: wh* finds what, white, and why, but not awhile or watch.</para>
    ///     <para>? - Matches a single alphabet in a specific position. Example: b?ll finds ball, bell, and bill.</para>
    /// </remarks>
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldMatchInContent(this HttpResponseMessage? actual, string expectedWildcardText, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(expectedWildcardText, nameof(expectedWildcardText), "Cannot verify an HTTP actual content match a <null> wildcard pattern.");

        ExecuteAssertion
            .ForCondition(actual is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        var content = actual!.GetContent();

        if (string.IsNullOrEmpty(content))
        {
            ExecuteAssertion
                .ForCondition(false)    
                .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {context:actual} to match the wildcard pattern {0} in its content, but content was <null>{reason}. {1}",
                        expectedWildcardText,
                        new FormatHttpResponseMessage(actual));
        }

        Action<string> assertionScope = (content) => {
            content!.ShouldMatch(expectedWildcardText);
        };
        var failures = assertionScope!.CollectFailuresFromAssertion(content);

        ExecuteAssertion
                   .ForCondition(failures.Length == 0)
                   .BecauseOf(because, becauseArgs)
                   .FailWith("Expected {context:actual} to match a wildcard pattern in its content, but does not since:{0}{reason}. {1}",
                       new AssertionsFailures(failures),
                       new FormatHttpResponseMessage(actual));
    }
}