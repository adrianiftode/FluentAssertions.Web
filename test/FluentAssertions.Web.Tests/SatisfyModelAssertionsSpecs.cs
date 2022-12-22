using System;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests;

public class SatisfyModelAssertionsSpecs
{
    #region Typed Model
    private class Model
    {
        public string? Property { get; set; }
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_twice_satisfy_assertion_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty())
            .And.Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_named_tuple_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<(string Property, object _)>(
                model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
    }


    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_non_named_tuple_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<Tuple<string, string>>(
                model => model.Item1.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().BeEmpty(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*to be empty, but found \"Value\"*HTTP response*");
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("\"True\"", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().BeNull(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*");
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_several_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model =>
                {
                    model.Property.Should().Be("Not Value");
                    model.Should().BeNull();
                }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*");
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot verify the subject satisfies a `null` assertion.*");
    }

    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(model => true.Should().BeTrue(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected a * to assert because we want to test the failure message, but found <null>.");
    }
    #endregion

    #region Inferred Model
    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_twice_satisfy_assertion_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty())
            .And.Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_given_model_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: (Model?)null, model => model!.Property.Should().NotBeNullOrEmpty());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().BeEmpty(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*to be empty, but found \"Value\"*HTTP response*");
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_several_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model =>
                  {
                      model.Property.Should().Be("Not Value");
                      model.Should().BeNull();
                  }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*");
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("\"True\"", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().BeNull(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*");
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot verify the subject satisfies a `null` assertion.*");
    }



    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => true.Should().BeTrue(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected a * to assert because we want to test the failure message, but found <null>.");
    }
    #endregion
}