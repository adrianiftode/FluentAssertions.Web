namespace Shouldly.Web.Tests.Internal;

/// <summary>
/// Every ShouldHaveHeaderX assertion fails with the "should have header" phrase (the presence
/// guard) when the header is absent, regardless of the value assertion that follows.
/// </summary>
public class HeaderPresenceTests
{
    public static TheoryData<Action<HttpResponseMessage>> Methods { get; } = new()
    {
        subject => subject.ShouldHaveHeaderWithValue("custom-header", "value1"),
        subject => subject.ShouldHaveHeaderWithValues("custom-header", new[] { "value1" }),
        subject => subject.ShouldHaveHeaderMatching("custom-header", "value*"),
        subject => subject.ShouldHaveEmptyHeader("custom-header"),
        subject => subject.ShouldHaveNonEmptyHeader("custom-header")
    };

    [Theory]
    [MemberData(nameof(Methods))]
    public void A_missing_header_fails_with_the_presence_phrase(Action<HttpResponseMessage> assertion)
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () => assertion(subject);

        // Assert
        act.ShouldFailWith("subject.Headers", "should have", "\"custom-header\"", "The HTTP response was:");
    }
}