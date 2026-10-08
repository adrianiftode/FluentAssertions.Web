using System.Net;
using System.Text;

namespace Assertions.Web.Tests.Internal;

public class ValidationErrorsTests
{
    private static HttpResponseMessage Response(string json)
        => new(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

    [Fact]
    public void When_the_body_has_an_errors_object_Fields_are_its_children()
    {
        // Arrange
        using var response = Response("""
            {
              "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
              "title": "One or more validation errors occurred.",
              "status": 400,
              "traceId": "00-deb7480af23a884e942f7b85cac6bd35-c1abe72874d40c4a-00",
              "errors": {
                "Author": [ "The Author field is required." ],
                "Comments": [ "The Comments field is required." ]
              }
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.Fields.Should().BeEquivalentTo(new[] { "Author", "Comments" });
        errors.Fields.Should().NotContain("type");
        errors.Fields.Should().NotContain("title");
        errors.Fields.Should().NotContain("status");
        errors.Fields.Should().NotContain("traceId");
    }

    [Fact]
    public void When_the_body_has_no_errors_object_Fields_are_the_root_children()
    {
        // Arrange
        using var response = Response("""
            {
              "Author": [ "The Author field is required." ]
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.Fields.Should().BeEquivalentTo(new[] { "Author" });
    }

    [Theory]
    [InlineData("Author", true)]
    [InlineData("author", true)]
    [InlineData("Comments", false)]
    public void HasField_compares_fields_case_insensitively(string field, bool expected)
    {
        // Arrange
        using var response = Response("""
            {
              "errors": {
                "Author": [ "The Author field is required." ]
              }
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.HasField(field).Should().Be(expected);
    }

    [Fact]
    public void MessagesOf_read_the_field_from_the_errors_object()
    {
        // Arrange
        using var response = Response("""
            {
              "errors": {
                "Author": [ "Message 1.", "Message 2." ]
              }
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.MessagesOf("Author").Should().BeEquivalentTo(new[] { "Message 1.", "Message 2." });
    }

    [Fact]
    public void MessagesOf_read_the_field_from_the_root_object()
    {
        // Arrange
        using var response = Response("""
            {
              "Author": "The Author field is required."
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.MessagesOf("Author").Should().BeEquivalentTo(new[] { "The Author field is required." });
    }

    [Fact]
    public void MessagesOf_wrap_a_single_string_value()
    {
        // Arrange
        using var response = Response("""
            {
              "errors": {
                "Author": "The Author field is required."
              }
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.MessagesOf("Author").Should().BeEquivalentTo(new[] { "The Author field is required." });
    }

    [Fact]
    public void AllMessages_aggregates_every_field_of_the_errors_object()
    {
        // Arrange
        using var response = Response("""
            {
              "errors": {
                "Author": [ "The Author field is required." ],
                "Comments": [ "The Comments field is required." ]
              }
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.AllMessages.Should().BeEquivalentTo(new[] { "The Author field is required.", "The Comments field is required." });
    }

    [Fact]
    public void AllMessages_aggregates_every_field_of_the_root_object()
    {
        // Arrange
        using var response = Response("""
            {
              "Author": [ "The Author field is required." ],
              "Comments": [ "The Comments field is required." ]
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.AllMessages.Should().BeEquivalentTo(new[] { "The Author field is required.", "The Comments field is required." });
    }

    [Fact]
    public void AllMessages_includes_the_empty_named_field()
    {
        // Arrange
        using var response = Response("""
            {
              "": [ "A non-empty request body is required." ]
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.AllMessages.Should().BeEquivalentTo(new[] { "A non-empty request body is required." });
    }

    [Fact]
    public void SiblingsOf_returns_the_errors_object_children()
    {
        // Arrange
        using var response = Response("""
            {
              "errors": {
                "Author": [ "The Author field is required." ],
                "Comments": [ "The Comments field is required." ]
              }
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.SiblingsOf("Author").Should().BeEquivalentTo(new[] { "Author", "Comments" });
    }

    [Fact]
    public void SiblingsOf_without_an_errors_object_returns_the_root_children()
    {
        // Arrange
        using var response = Response("""
            {
              "Author": [ "The Author field is required." ]
            }
            """);

        // Act
        using var errors = ValidationErrors.Read(response);

        // Assert
        errors.SiblingsOf("Author").Should().BeEquivalentTo(new[] { "Author" });
    }
}