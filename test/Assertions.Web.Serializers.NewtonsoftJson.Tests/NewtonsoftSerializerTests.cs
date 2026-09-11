namespace Assertions.Web.Serializers.NewtonsoftJson.Tests;

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
        // Act
        Action act = () =>
            subject.ShouldBeAs(new
            {
                accepted = true,
                required = false
            });

        // Assert
        act.ShouldNotThrow();
#else
       // Act
        Action act = () =>
            subject.Should().BeAs(new
            {
                accepted = true,
                required = false
            });

        // Assert
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

#if SH
        // Act
        Action act = () =>
             subject.ShouldBeAs(new
             {
                 accepted = false
             });

        // Assert
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch(@"(?s).*value to be\s+False\s+but was\s+True.*");
#else
        // Act
        Action act = () =>
             subject.Should().BeAs(new
             {
                 accepted = false
             });

        // Assert
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

#if SH
        // Act
        Action act = () =>
             subject.ShouldBeAs(new
             {
                 accepted = false
             });

        // Assert
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)but the JSON representation(.*)NewtonsoftJsonSerializer(.*)Error converting value "da" to type 'System.Boolean'(.*)""");
#else
        // Act
        Action act = () =>
             subject.Should().BeAs(new
             {
                 accepted = false
             });

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""*but the JSON representation*NewtonsoftJsonSerializer*Error converting value "da" to type 'System.Boolean'*""");
#endif
    }

    [Fact(Skip = "See issue #160")]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_named_tuple_type_it_should_succeed()
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
                model => model.Property.ShouldNotBeNullOrEmpty());

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<(string Property, object _)>(
                model => model.Property.Should().NotBeNullOrEmpty());

        // Assert
        act.Should().NotThrow();
#endif
    }

    [Fact(Skip = "See issue #160")]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_non_named_tuple_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

        // Act
#if SH
        Action act = () =>
            subject.ShouldSatisfy<Tuple<string, string>>(
                model => model.Item1.ShouldNotBeNullOrEmpty());

        // Assert
        act.ShouldNotThrow();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<Tuple<string, string>>(
                model => model.Item1.Should().NotBeNullOrEmpty());

        // Assert
        act.Should().NotThrow();
#endif
    }
}
