namespace Assertions.Web.Tests;

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

        // Act
        Action act = () =>
#if SH
        subject.ShouldSatisfy<Model>(model =>
        {
            model.Property.ShouldNotBeEmpty();
            model.Property.ShouldNotBeEmpty();
        });
#else
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty())
            .And.Satisfy<Model>(
                model => model.Property.Should().NotBeEmpty());
#endif

        // Assert
#if SH
        act.ShouldNotThrow();
#else
        act.Should().NotThrow();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_named_tuple_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<(string Property, object _)>(
                model => model.Property.ShouldNotBeEmpty());
#else
            subject.Should().Satisfy<(string Property, object _)>(
                model => model.Property.Should().NotBeEmpty());
#endif
        };

        // Assert
#if SH
        act.ShouldNotThrow();
#else
        act.Should().NotThrow();
#endif
    }


    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertion_and_model_is_of_non_named_tuple_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<Tuple<string, string>>(
                model => model.Item1.ShouldNotBeEmpty());
#else
            subject.Should().Satisfy<Tuple<string, string>>(
                model => model.Item1.Should().NotBeEmpty());
#endif
        };

        // Assert
#if SH
        act.ShouldNotThrow();
#else
        act.Should().NotThrow();
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldBeEmpty(), "we want to test the {0}", "reason");
#else
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().BeEmpty(), "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)Expected (.*) to satisfy one or more model assertions, but it wasn't because we want to test the reason:(.*)expected(.*)to be empty, but found "Value"(.*)HTTP response(.*)""");
#else
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
            Content = new StringContent("""        "True"        """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<Model>(
                model => model.Property.ShouldBeNull(), "we want to test the {0}", "reason");
#else
            subject.Should().Satisfy<Model>(
                model => model.Property.Should().BeNull(), "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)to have a content equivalent to a model of type(.*), but the JSON representation could not be parsed(.*)""");
#else
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<Model>(
                model =>
                {
                    model.Property.ShouldBe("Not Value");
                    model.ShouldBeNull();
                }, "we want to test the {0}", "reason");
#else
            subject.Should().Satisfy<Model>(
                model =>
                {
                    model.Property.Should().Be("Not Value");
                    model.Should().BeNull();
                }, "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)Expected (.*) to satisfy one or more model assertions, but it wasn't because we want to test the reason:(.*)expected(.*)Not Value(.*)expected(.*)to be null(.*)The HTTP response was:(.*)""");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*""");
#endif
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_assertion_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<Model>(null!);
#else
            subject.Should().Satisfy<Model>(null!);
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldMatch(@"Cannot verify the subject satisfies a `null` assertion\.(.*)");
#else
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot verify the subject satisfies a `null` assertion.*");
#endif
    }

    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy<Model>(model => true.ShouldBeTrue(), "because we want to test the failure {0}", "message");
#else
            subject.Should().Satisfy<Model>(model => true.Should().BeTrue(), "because we want to test the failure {0}", "message");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch(@"Expected a (.*) to assert because we want to test the failure message, but found <null>\.");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldNotBeEmpty());
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty());
#endif
        };

        // Assert
#if SH
        act.ShouldNotThrow();
#else
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldNotBeEmpty());
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldNotBeEmpty());
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty())
            .And.Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().NotBeEmpty());
#endif
        };

        // Assert
#if SH
        act.ShouldNotThrow();
#else
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: (Model?)null, model => model!.Property.ShouldNotBeNullOrEmpty());
#else
            subject.Should().Satisfy(givenModelStructure: (Model?)null, model => model!.Property.Should().NotBeNullOrEmpty());
#endif
        };

        // Assert
#if SH
        act.ShouldNotThrow();
#else
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldBeEmpty(), "we want to test the {0}", "reason");
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().BeEmpty(), "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch("""(.*)Expected (.*) to satisfy one or more model assertions, but it wasn't because we want to test the reason:(.*)expected(.*)to be empty, but found "Value"(.*)HTTP response(.*)""");
#else
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

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model =>
            {
                model.Property.ShouldBe("Not Value");
                model.ShouldBeNull();
            }, "we want to test the {0}", "reason");
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model =>
                  {
                      model.Property.Should().Be("Not Value");
                      model.Should().BeNull();
                  }, "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch(@"Expected (.*) to satisfy one or more model assertions, but it wasn't because we want to test the reason:(.*)expected(.*)Not Value(.*)expected(.*)to be null(.*)The HTTP response was:(.*)");
#else
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
            Content = new StringContent("""        "True"        """, Encoding.UTF8, "application/json")
        };

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.ShouldBeNull(), "we want to test the {0}", "reason");
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => model.Property.Should().BeNull(), "we want to test the {0}", "reason");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch(@"(.*)to have a content equivalent to a model of type(.*), but the JSON representation could not be parsed(.*)");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("*to have a content equivalent to a model of type*, but the JSON representation could not be parsed*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_assertion_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage();

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, null!);
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, null!);
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldMatch(@"Cannot verify the subject satisfies a `null` assertion\.(.*)");
#else
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Cannot verify the subject satisfies a `null` assertion.*");
#endif
    }

    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
        {
#if SH
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => true.ShouldBeTrue(), "because we want to test the failure {0}", "message");
#else
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: model => true.Should().BeTrue(), "because we want to test the failure {0}", "message");
#endif
        };

        // Assert
#if SH
        act.ShouldThrow<ShouldAssertException>()
            .Message.ShouldMatch(@"Expected a (.*) to assert because we want to test the failure message, but found <null>\.");
#else
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
    #endregion
}