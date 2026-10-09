#if SH
namespace Shouldly.Web.Serializers.NewtonsoftJson.Tests;
#elif AAV
namespace AwesomeAssertions.Web.Serializers.NewtonsoftJson.Tests;
#else
namespace FluentAssertions.Web.Serializers.NewtonsoftJson.Tests;
#endif

public class NewtonsoftSerializerTests
{
    [Fact]
    public void When_asserting_response_with_content_convertible_using_Newtonsoft_Json_Converters_to_be_as_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json*/"""
                {
                  "accepted": "yes",
                  "required": "no"
                }
                """, Encoding.UTF8, "application/json")
        };

#if SH
        subject.ShouldBeAs(new
        {
            accepted = true,
            required = false
        });
#else
        subject.Should().BeAs(new
        {
            accepted = true,
            required = false
        });
#endif

        // Act
        Action act = () =>
#if SH
            subject.ShouldBeAs(new
            {
                accepted = true,
                required = false
            });
#else
            subject.Should().BeAs(new
            {
                accepted = true,
                required = false
            });
#endif

        // Assert
#if SH
        act.ShouldNotThrow();
#else
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_with_content_with_differences_using_Newtonsoft_Json_Converters_to_be_as_model_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """
                {
                  "accepted": "yes"
                }
                """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
#if SH
            subject.ShouldBeAs(new
            {
                accepted = false
            });
#else
             subject.Should().BeAs(new
             {
                 accepted = false
             });
#endif

        // Assert
#if SH
        var message = act.ShouldThrow<ShouldAssertException>().Message;
        message.ShouldContain("Comparing object equivalence");
        message.ShouldContain("accepted [System.Boolean]");
        message.ShouldContain("but was");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("*accepted to be False, but found True*");
#endif
    }

    [Fact]
    public void When_asserting_response_with_content_with_not_convertible_value_using_Newtonsoft_Json_Converters_to_be_as_model_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """
                {
                  "accepted": "da"
                }
                """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
#if SH
            subject.ShouldBeAs(new
            {
                accepted = false
            });
#else
             subject.Should().BeAs(new
             {
                 accepted = false
             });
#endif

        // Assert
#if SH
        var message = act.ShouldThrow<ShouldAssertException>().Message;
        message.ShouldContain("Exception while deserializing the model with NewtonsoftJsonSerializer");
        message.ShouldContain("Error converting value \"da\" to type 'System.Boolean'");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("""*but the JSON representation*NewtonsoftJsonSerializer*Error converting value "da" to type 'System.Boolean'*""");
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

        // Act
        Action act = () =>
#if SH
            subject.ShouldSatisfy<(string Property, object _)>(
                model => model.Property.ShouldNotBeNullOrEmpty(), "because we want to test the reason");
#else
            subject.Should().Satisfy<(string Property, object _)>(
                model => model.Property.Should().NotBeNullOrEmpty(), "because we want to test the {0}", "reason");
#endif

        // Assert
#if SH
        var message = act.ShouldThrow<ShouldAssertException>().Message;
        message.ShouldContain("exist only at compile time and are absent from the runtime type");
        message.ShouldContain("because we want to test the reason");
#else
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

        // Act
        Action act = () =>
#if SH
            subject.ShouldSatisfy<Tuple<string, string>>(
                model => model.Item1.ShouldNotBeNullOrEmpty(), "because we want to test the reason");
#else
            subject.Should().Satisfy<Tuple<string, string>>(
                model => model.Item1.Should().NotBeNullOrEmpty(), "because we want to test the {0}", "reason");
#endif

        // Assert
#if SH
        var message = act.ShouldThrow<ShouldAssertException>().Message;
        message.ShouldContain("exist only at compile time and are absent from the runtime type");
        message.ShouldContain("because we want to test the reason");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type *System.Tuple`2*, but the JSON representation could not be parsed*" +
                "*exist only at compile time and are absent from the runtime type*because we want to test the reason*");
#endif
    }
}
