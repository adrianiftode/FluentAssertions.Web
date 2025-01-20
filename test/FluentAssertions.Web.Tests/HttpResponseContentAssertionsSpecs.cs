namespace FluentAssertions.Web.Tests;

public class HttpResponseContentAssertionsSpecs
{
    #region BeEmpty

    [Fact]
    public void When_asserting_response_with_no_content_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () => subject.Should().BeEmpty();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_empty_content_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage()
        {
            Content = new StringContent("")
        };

        // Act
        Action act = () => subject.Should().BeEmpty();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_content_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
                                                     {
                                                       "comment": "Hey",
                                                       "author": "John"
                                                     }
                                                     """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () => subject.Should().BeEmpty("we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have no content*");
    }
    #endregion

    #region BeAs
    [Fact]
    public void When_asserting_response_with_content_to_be_as_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "comment": "Hey",
              "author": "John"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                Comment = "Hey",
                Author = "John"
            });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_content_with_more_JSON_properties_than_a_model_to_be_as_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "author": "John",
              "comment": "Hey"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                Comment = "Hey",
            });

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_content_with_differences_to_be_as_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "comment": "Hey",
              "author": "John"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                Comment = "Not Hey",
                Author = "John"
            }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Not Hey*with a length*reason*");
    }

    public static IEnumerable<object[]> Data =>
       new List<object[]>
       {
            new object[] { 1 },
            new object[] { "" },
            new object[] { new DateTime(2019, 10, 11) }
       };
    [Theory]
    [MemberData(nameof(Data))]
    public void When_asserting_response_with_content_as_primitives_types_to_be_as_the_primitive_value_it_should_succeed(object expectedModel)
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(expectedModel.ToJson(), Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(expectedModel);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_content_as_int_to_be_as_the_primitive_value_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(1.ToJson(), Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(1);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_fewer_JSON_properties_than_the_model_to_be_as_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "comment": "Hey"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                Comment = "Hey",
                Author = "John"
            }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""*Author to be "John"*reason*""");
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_be_as_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("some text:that doesn't look like a json {", Encoding.UTF8, "text/plain")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                Comment = "Hey"
            }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*");
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_be_as_model_it_should_throw_with_descriptive_message_which_includes_the_parsing_error_details()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "price" : 0.0}""", Encoding.UTF8, "text/plain")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                price = 0
            }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""*to have a content equivalent to a model of type*, but the JSON representation could not be parsed, as the operation failed with the following message: "Exception while deserializing the model with SystemTextJsonSerializer: The JSON value could not be converted to * Path: $.price | LineNumber: 0 | BytePositionInLine: 15.*""");
    }

    [Fact]
    public void When_asserting_response_with_a_syntactically_correct_JSON_array_to_be_as_a_single_object_model_it_should_throw_with_descriptive_message_which_includes_the_serialization_error_details_but_keep_the_original_content()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """[{ "price" : 0.0}, { "price" : 1.0}]""", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                price = 0m
            }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""
            *to have a content equivalent to a model of type*, but the JSON representation could not be parsed, as the operation failed with the following message: "Exception while deserializing the model with SystemTextJsonSerializer: The JSON value could not be converted to * Path: $ | LineNumber: 0 | BytePositionInLine: 1.*
            [
              {
                "price": 0.0
              },
              {
                "price": 1.0
              }
            ]*
            """);
    }

    [Fact]
    public void When_asserting_with_equivalency_assertion_options_to_be_as_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """
            {
              "author": "John",
              "comment": "Hey",
              "version": "version 1"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                author = "John",
                comment = "Hey",
                version = "version 2"
            }, options => options.Excluding(model => model.version));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_with_equivalency_assertion_options_and_with_differences_to_be_as_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "comment": "Hey",
              "author": "John"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                Comment = "Not Hey",
                Author = "John"
            }, options => options.Excluding(model => model.Author), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*Not Hey*with a length*reason*");
    }

    [Fact]
    public void When_asserting_response_to_be_as_model_with_null_equivalency_assertion_options_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().BeAs(new { }, options: (Func<EquivalencyOptions<object>, EquivalencyOptions<object>>)(null!));

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null*options*");
    }

    [Fact]
    public void When_asserting_response_to_be_as_against_null_value_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().BeAs((object?)null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*content*<null>*");
    }

    [Fact]
    public void When_asserting_null_response_to_be_as_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
            subject.Should().BeAs((object?)null, "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
    }
    #endregion

    #region MatchInContent
    [Theory]
    [InlineData("*comment*author*")]
    [InlineData("*co?ment*a?thor*")]
    public void When_asserting_response_with_keywords_in_content_to_match_in_content_it_should_succeed(string wildcardText)
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "comment": "Hey",
              "author": "John"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().MatchInContent(wildcardText);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_without_keywords_in_content_to_match_in_content_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
            {
              "comment": "Hey",
              "author": "John"
            }
            """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().MatchInContent("*notes*", "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*notes*message*");
    }

    [Fact]
    public void When_asserting_response_with_no_string_content_to_match_in_content_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().MatchInContent("*notes*", "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*the wildcard pattern*notes*content was <null>*message*");
    }

    [Fact]
    public void When_asserting_response_to_match_in_content_against_null_wildcard_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().MatchInContent(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*<null>*wildcard*");
    }

    [Fact]
    public void When_asserting_null_response_to_match_in_content_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
            subject.Should().MatchInContent("wildcard", "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
    }
    #endregion
}
