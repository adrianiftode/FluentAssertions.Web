namespace FluentAssertions.Web.Tests;

public class HaveLocationHeaderSpecs
{
    #region MovedPermanently
    [Fact]
    public void When_asserting_moved_permanently_response_with_location_header_to_be_Be301MovedPermanently_to_a_specific_location_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.MovedPermanently)
        {
            Headers = { { "Location", "/new-location" } }
        };

        // Act 
        Action act = () =>
            subject.Should().Be301MovedPermanently()
                .And.HaveLocationHeader("/new-location");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_moved_permanently_response_with_location_header_to_be_Be301MovedPermanently_to_a_specific_location_it_should_throw_with_descriptive_messaged()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.MovedPermanently)
        {
            Headers = { { "Location", "/new-location" } }
        };

        // Act 
        Action act = () =>
            subject.Should().Be301MovedPermanently()
                .And.HaveLocationHeader("/other-location");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to contain the error message "One or more validation errors occurred.", but no such message was found in the actual error messages list*""");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void When_asserting_moved_permanently_response_with_location_header_to_be_Be301MovedPermanently_to_a_specific_location_against_null_or_empty_string_value_for_the_location_header_it_should_throw_with_descriptive_message(string expectedLocation)
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.MovedPermanently)
        {
            Headers = { { "Location", "/new-location" } }
        };

        // Act
        Action act = () => subject.Should().Be301MovedPermanently()
            .And.HaveLocationHeader(expectedLocation);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*<null> or empty wildcard error message*");
    }
    #endregion
}
