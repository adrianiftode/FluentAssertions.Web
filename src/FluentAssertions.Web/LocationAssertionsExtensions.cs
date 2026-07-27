#if !FAV8

namespace FluentAssertions;


/// <summary>
/// Contains extension methods for custom assertions in unit tests related to <see cref="LocationAssertions"/>.
/// </summary>
[DebuggerNonUserCode]
public static class LocationAssertionsExtensions
{
    /// <summary>
    /// Asserts that an HTTP response has a Location header.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HeadersAssertions> HaveLocation(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string because = "", params object[] becauseArgs)
    => new LocationAssertions(parent.Subject).HaveLocation(because, becauseArgs);
}
#endif