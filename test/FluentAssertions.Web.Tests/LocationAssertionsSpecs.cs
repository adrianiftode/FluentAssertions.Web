#if AAV
namespace AwesomeAssertions.Web.Tests;
#else
namespace FluentAssertions.Web.Tests;
#endif

public class LocationAssertionsSpecs
{
    #region HaveLocation
    [Fact]
    public void When_asserting_201_created_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Created)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be201Created().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData((string?)null)]
    public void When_asserting_201_created_response_with_location_header_and_no_header_value_to_have_the_location_header_it_should_succeed(string? locationValue)
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Created);
        subject.Headers.TryAddWithoutValidation("Location", locationValue);

        // Act
        Action act = () =>
            subject.Should().Be201Created().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_300_ambiguous_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Ambiguous)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be300Ambiguous().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_301_moved_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Moved)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be301Moved().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_301_moved_permanently_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.MovedPermanently)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be301MovedPermanently().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_302_found_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Found)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be302Found().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_303_see_other_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.SeeOther)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be303SeeOther().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_305_use_proxy_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.UseProxy)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be305UseProxy().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_307_temporary_redirect_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.TemporaryRedirect)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be307TemporaryRedirect().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_307_redirect_keep_verb_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.RedirectKeepVerb)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be307RedirectKeepVerb().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_308_redirect_keep_verb_response_with_location_header_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.RedirectKeepVerb)
        {
            Headers =
            {
                { "Location", "1.html" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().Be308PermanentRedirect().And.HaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_201_created_response_without_location_header_to_have_the_location_header_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Created);

        // Act
        Action act = () =>
            subject.Should().Be201Created().And.HaveLocation("we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to contain the Location HTTP header, but no such header was found in the actual response*reason*.");
    }
    #endregion

    #region NotHaveLocation
    [Fact]
    public void When_asserting_a_response_without_a_location_header_not_to_have_the_location_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.OK);

        // Act
        Action act = () =>
            subject.Should().NotHaveLocation();

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData((string?)null)]
    public void When_asserting_a_response_with_location_header_and_no_header_value_to_not_have_the_location_header_it_should_throw_with_descriptive_message(string? locationValue)
    {
        // Arrange
        using var subject = new HttpResponseMessage(HttpStatusCode.Created);
        subject.Headers.TryAddWithoutValidation("Location", locationValue);

        // Act
        Action act = () =>
            subject.Should().Be201Created().And.NotHaveLocation("we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected subject to not contain the Location HTTP header, but the header was found in the actual response*reason*.");
    }

    #endregion
}
