// ReSharper disable once CheckNamespace
namespace FluentAssertions;

/// <summary>
/// Contains extension methods for custom assertions in unit tests related to <see cref="HaveLocationHeaderAssertions"/>.
/// </summary>
[DebuggerNonUserCode]
public static class HaveLocationHeaderAssertionsExtensions
{
    /// <summary>
    /// Asserts that an HTTP response has a named header.
    /// </summary>
    /// <param name="expectedHeader">
    /// The expected header with which the HTTP headers list is matched.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HaveLocationHeaderAssertions> HaveLocationHeader(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string expectedHeader,
        string because = "", params object[] becauseArgs)
    => new HaveLocationHeaderAssertions(parent.Subject).HaveLocationHeader(expectedHeader, because, becauseArgs);

    /// <summary>
    /// Asserts that an HTTP response does not have a named header.
    /// </summary>
    /// <param name="expectedHeader">
    /// The expected header with which the HTTP headers list is matched.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HaveLocationHeaderAssertions> NotHaveLocationHeader(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string expectedHeader,
        string because = "", params object[] becauseArgs)
        => new HaveLocationHeaderAssertions(parent.Subject).NotHaveLocationHeader(expectedHeader, because, becauseArgs);
}