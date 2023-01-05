namespace FluentAssertions.Web;

/// <summary>
/// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state related to HTTP response that are expected to contain the Location header
/// </summary>
public class HaveLocationHeaderAssertions : HeadersAssertions
{
    /// <summary>
    /// Initialized a new instance of the <see cref="HaveLocationHeaderAssertions"/>
    /// class.
    /// </summary>
    /// <param name="value">The subject value to be asserted.</param>
    public HaveLocationHeaderAssertions(HttpResponseMessage value) : base(value, "Location")
    {
    }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "HaveLocationHeader";

    /// <summary>
    /// Asserts that the location header of the Redirect HTTP response is the expected value.
    /// </summary>
    /// <remarks>
    /// This assertion considers the HTTP response is a of a Redirect type.
    /// </remarks>
    /// <param name="expectedLocationHeaderValue">
    /// The expected value of the Location header.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public AndConstraint<HaveLocationHeaderAssertions> HaveLocationHeader(string expectedLocationHeaderValue,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(expectedLocationHeaderValue, nameof(expectedLocationHeaderValue), "Cannot verify a Location HTTP header to be a value against a <null> or empty value.");

        Subject.Should().HaveHeader("Location", because, becauseArgs).And.BeValue(expectedLocationHeaderValue, because, becauseArgs);

        return new AndConstraint<HaveLocationHeaderAssertions>(this);
    }

    /// <summary>
    /// Asserts that the location header of the Redirect HTTP response is the expected value.
    /// </summary>
    /// <remarks>
    /// This assertion considers the HTTP response is a of a Redirect type.
    /// </remarks>
    /// <param name="expectedLocationHeaderValue">
    /// The expected value of the Location header.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public AndConstraint<HaveLocationHeaderAssertions> NotHaveLocationHeader(string expectedLocationHeaderValue,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(expectedLocationHeaderValue, nameof(expectedLocationHeaderValue), "Cannot verify a Location HTTP header to be a value against a <null> value.");

        Subject.Should().NotHaveHeader("Location", because, becauseArgs);

        return new AndConstraint<HaveLocationHeaderAssertions>(this);
    }
}