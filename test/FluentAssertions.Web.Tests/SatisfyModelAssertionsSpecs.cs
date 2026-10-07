#if SH
namespace Shouldly.Web.Tests;
#elif AAV
namespace AwesomeAssertions.Web.Tests;
#else
namespace FluentAssertions.Web.Tests;
#endif

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
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldNotBeEmpty());

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_twice_satisfy_assertion_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
        {
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldNotBeEmpty());
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldNotBeEmpty());
        };

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty())
            .And.Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_named_tuple_type_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<(string Property, object _)>(
                model => model.Property.ShouldNotBeNullOrEmpty(), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "System.ValueTuple", "exist only at compile time and are absent from the runtime type", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<(string Property, object _)>(
                model => model.Property.Should().NotBeNullOrEmpty(), "because we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type *System.ValueTuple`2*, but the JSON representation could not be parsed*" +
                "*exist only at compile time and are absent from the runtime type*because we want to test the reason*");
#endif
    }


    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_non_named_tuple_type_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Tuple<string, string>>(
                model => model.Item1.ShouldNotBeNullOrEmpty(), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "System.Tuple", "exist only at compile time and are absent from the runtime type", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Tuple<string, string>>(
                model => model.Item1.Should().NotBeNullOrEmpty(), "because we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type *System.Tuple`2*, but the JSON representation could not be parsed*" +
                "*exist only at compile time and are absent from the runtime type*because we want to test the reason*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldBeEmpty(), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "model.Property", "should be empty but was", "\"Value\"", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().BeEmpty(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*to be empty, but found "Value"*HTTP response*""");
#endif
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("""
            "True"
            """, Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldBeNull(), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "Exception while deserializing the model with SystemTextJsonSerializer", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().BeNull(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_several_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Shouldly stops at the first failure; nest ShouldSatisfy to report all.
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>(model => model.ShouldSatisfy([
                m => m.Property.ShouldBe("Not Value"),
                m => m.ShouldBeNull()]), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "m.Property", "should be", "\"Not Value\"", "but was", "\"Value\"", "Error 2", "should be null but was", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
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
            .WithMessage("""Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*""");
#endif
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>((Action<Model>)null!);

        // Assert
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot verify the subject satisfies a `null` assertion.*");
#endif
    }

    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>(model => true.ShouldBeTrue(), "because we want to test the failure message");

        // Assert
        act.ShouldFailWith("subject", "should not be null", "Additional Info:", "because we want to test the failure message");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(model => true.Should().BeTrue(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
    #endregion

#region Named Tuple Model
    [Fact]
    public void When_asserting_response_content_to_be_equivalent_to_a_named_tuple_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldBeAs((Property: "Value", Other: (object?)null));

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "exist only at compile time and are absent from the runtime type", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().BeAs((Property: "Value", Other: (object?)null));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*but the JSON representation could not be parsed*" +
                "*exist only at compile time and are absent from the runtime type*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_leading_whitespace_to_satisfy_assertion_on_a_tuple_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """
                  
                  { "property" : "Value" }  
                
                """, Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<(string Property, object _)>(model => true.ShouldBeTrue());

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "exist only at compile time and are absent from the runtime type", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<(string Property, object _)>(model => true.Should().BeTrue());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*exist only at compile time and are absent from the runtime type*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_a_json_array_to_satisfy_assertion_on_a_tuple_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """[ "Value", 42 ]""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<(string, object)>(model => true.ShouldBeTrue());

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "Exception while deserializing the model with SystemTextJsonSerializer", "could not be converted to System.ValueTuple", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<(string, object)>(model => true.Should().BeTrue());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*" +
                "*Exception while deserializing the model with SystemTextJsonSerializer*could not be converted to System.ValueTuple*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_assertion_on_a_model_with_named_members_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<Model>(model => model.Property.ShouldBe("Value"));

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Model>(model => model.Property.Should().Be("Value"));

        // Assert
        act.Should().NotThrow();
#endif
    }
    #endregion

    #region Inferred Model
    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldNotBeEmpty());

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty());

        // Assert
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_twice_satisfy_assertion_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
        {
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldNotBeEmpty());
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldNotBeEmpty());
        };

        // Assert
        act.ShouldNotThrow();
#else
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
#endif
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_given_model_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: (Model?)null, model => model!.Property.ShouldNotBeNullOrEmpty());

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: (Model?)null, model => model!.Property.Should().NotBeNullOrEmpty());

        // Assert
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldBeEmpty(), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "model.Property", "should be empty but was", "\"Value\"", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().BeEmpty(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*to be empty, but found "Value"*HTTP response*""");
#endif
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_several_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Shouldly stops at the first failure; nest ShouldSatisfy to report all.
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.ShouldSatisfy([
                m => m.Property.ShouldBe("Not Value"),
                m => m.ShouldBeNull()]), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "m.Property", "should be", "\"Not Value\"", "but was", "\"Value\"", "Error 2", "should be null but was", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
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
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*");
#endif
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("""
            "True"
            """, Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldBeNull(), "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "Exception while deserializing the model with SystemTextJsonSerializer", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().BeNull(), "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

#if SH
        // The anonymous structure cannot infer a delegate type for a bare null; use an explicit structure.
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: (Model)null!, assertion: (Action<Model>)null!);

        // Assert
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot verify the subject satisfies a `null` assertion.*");
#endif
    }

    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => true.ShouldBeTrue(), "because we want to test the failure message");

        // Assert
        act.ShouldFailWith("subject", "should not be null", "Additional Info:", "because we want to test the failure message");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => true.Should().BeTrue(), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
    #endregion
}