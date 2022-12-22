using System;
using System.Linq;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests;

public class HeadersAssertionsSpecs
{
    #region HaveHeader
    [Fact]
    public void When_asserting_response_with_header_to_have_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_header_and_no_header_value_to_have_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", (string?)null }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_no_headers_to_have_header_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header", "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*the HTTP header*custom-header*but no such header was found*reason*");
    }

    [Fact]
    public void When_asserting_response_without_header_to_have_header_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "other-header", "other-header-value" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header", "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*the HTTP header*custom-header*but no such header was found*reason*");
    }

    [Fact]
    public void When_asserting_response_to_have_header_against_null_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().HaveHeader(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*header*<null>*");
    }

    #endregion

    #region NotHaveHeader
    [Fact]
    public void When_asserting_response_with_headers_to_to_not_have_a_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "a-header", "with-a-value" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().NotHaveHeader("other-header");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_no_headers_to_not_to_have_header_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().NotHaveHeader("custom-header");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_a_header_to_not_have_that_header_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "that-header", "with-a-value" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().NotHaveHeader("that-header", "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to not to contain the HTTP header*that-header*but the header was found*reason*");
    }

    [Fact]
    public void When_asserting_response_to_not_have_header_against_null_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().NotHaveHeader(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*not having*header*<null>*");
    }

    #endregion

    #region BeEmpty
    [Fact]
    public void When_asserting_response_with_header_and_no_header_value_to_have_header_and_be_empty_value_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", (string?)null }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.BeEmpty();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_header_and_a_header_value_to_have_that_header_and_be_empty_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "some-non-empty-value" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.BeEmpty("we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*the HTTP header *custom-header* with no header values*some-non-empty-value*reason*");
    }
    #endregion

    #region Match
    [Fact]
    public void When_asserting_response_with_header_to_have_header_with_a_value_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.Match("value*");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_header_without_a_value_to_have_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.Match("other-than-value1", "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*the HTTP header*custom-header* having a value matching *other-than-value1*, but there was no match*reason*");
    }

    [Fact]
    public void When_asserting_response_to_have_header_and_be_value_against_null_expected_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.Match(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*<null>*Use And.BeEmpty to test if the HTTP header has no values.*");
    }
    #endregion

    #region BeValues
    [Fact]
    public void When_asserting_response_with_header_with_values_to_have_those_values_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" },
                { "custom-header", "value2" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.BeValues(new[] { "value1", "value2" });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_header_without_some_value_to_be_different_values_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.BeValues(new[] {
                "other-than-value1",
                "another-other-than-value1" }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*the HTTP header*custom-header*having values*other-than-value1*another-other-than-value1*reason*");
    }

    [Fact]
    public void When_asserting_response_to_have_header_and_be_values_against_null_expected_values_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.BeValues(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*<null>*Use And.BeEmpty to test if the HTTP header has no values.*");
    }

    [Fact]
    public void When_asserting_response_to_have_header_and_be_values_against_empty_expected_values_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Headers =
            {
                { "custom-header", "value1" }
            }
        };

        // Act
        Action act = () =>
            subject.Should().HaveHeader("custom-header").And.BeValues(Enumerable.Empty<string>());

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*empty*Use And.BeEmpty to test if the HTTP header has no values.*");
    }
    #endregion
}