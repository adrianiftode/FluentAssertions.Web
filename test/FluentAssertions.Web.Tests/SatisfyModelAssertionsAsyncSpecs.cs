#if SH
using Shouldly.Web.Tests.TestModels;
#elif AAV
using AwesomeAssertions.Web.Tests.TestModels;
#else
using FluentAssertions.Web.Tests.TestModels;
#endif

#if SH
namespace Shouldly.Web.Tests;
#elif AAV
namespace AwesomeAssertions.Web.Tests;
#else
namespace FluentAssertions.Web.Tests;
#endif

public class SatisfyModelAssertionsAsyncSpecs
{
    #region Typed Model

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertions_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.Property.ShouldNotBeEmpty();
                    completed = true;
                });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.Property.Should().NotBeEmpty();
                    completed = true;
                });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.Property.ShouldBeEmpty();
                }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "model.Property", "should be empty but was", "\"Value\"", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.Property.Should().BeEmpty();
                }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("""Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*to be empty, but found "Value"*HTTP response*""");
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_strings_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : "Type1"}""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.ShouldBe(TestEnum.Type1);
                    completed = true;
                });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.Should().Be(TestEnum.Type1);
                    completed = true;
                });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : 2 }""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.ShouldBe(TestEnum.Type1);
                    completed = true;
                });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.Should().Be(TestEnum.Type1);
                    completed = true;
                });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_with_no_values_in_range_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : -1 }""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.ShouldBe((TestEnum)(-1));
                    completed = true;
                });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.Should().Be((TestEnum)(-1));
                    completed = true;
                });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : -1 }""", Encoding.UTF8, "application/json")
        };

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.ShouldBe(TestEnum.Type1);
                }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "model.TestEnum", "should be", "TestEnum.Type1", "but was", "TestEnum.-1", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModelWithEnum>(
                async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.Should().Be(TestEnum.Type1);
                }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*the enum to be TestEnum.Type1*, but found TestEnum.-1**HTTP response*");
#endif
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertions_it_should_throw_with_descriptive_message()
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
            subject.ShouldSatisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.Property.ShouldBeNull();
                }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "Exception while deserializing the model with SystemTextJsonSerializer", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.Property.Should().BeNull();
                }, "we want to test the {0}", "reason");

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
            subject.ShouldSatisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
                    model.ShouldSatisfy([
                        m => m.Property.ShouldBe("Not Value"),
                        m => m.ShouldBeNull()]);
                }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "m.Property", "should be", "\"Not Value\"", "but was", "\"Value\"", "Error 2", "should be null but was", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
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
            subject.ShouldSatisfy<TestModel>((Func<TestModel, Task>)null!);

        // Assert
        act.ShouldThrow<ArgumentNullException>()
            .Message.ShouldStartWith("Cannot verify the subject satisfies a `null` assertion.");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>((Func<TestModel, Task>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
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
            subject.ShouldSatisfy<TestModel>(async model => await Task.Run(() => true.ShouldBeTrue()), "because we want to test the failure message");

        // Assert
        act.ShouldFailWith("subject", "should not be null", "Additional Info:", "because we want to test the failure message");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>(async model => await Task.Run(() => true.Should().BeTrue()), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
    #endregion

    #region Inferred Model
    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertions_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "property" : "Value"}""", Encoding.UTF8, "application/json")
        };
        var completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.ShouldNotBeEmpty();
                completed = true;
            });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion:async model =>
            {
                await Task.Delay(10);
                model.Property.Should().NotBeEmpty();
                completed = true;
            });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
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
        var completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: (TestModel?)null, async model =>
            {
                await Task.Delay(10);
                model!.Property.ShouldNotBeNullOrEmpty();
                completed = true;
            });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: (TestModel?)null, async model =>
            {
                await Task.Delay(10);
                model!.Property.Should().NotBeNullOrEmpty();
                completed = true;
            });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
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
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.ShouldBeEmpty();
            }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "model.Property", "should be empty but was", "\"Value\"", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.Should().BeEmpty();
            }, "we want to test the {0}", "reason");

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
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.ShouldSatisfy([
                    m => m.Property.ShouldBe("Not Value"),
                    m => m.ShouldBeNull()]);
            }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "m.Property", "should be", "\"Not Value\"", "but was", "\"Value\"", "Error 2", "should be null but was", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: async model =>
                  {
                      await Task.Delay(10);
                      model.Property.Should().Be("Not Value");
                      model.Should().BeNull();
                  }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*");
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_strings_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : "Type1"}""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                TestEnum = TestEnum.Type1
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.TestEnum.ShouldBe(TestEnum.Type1);
                completed = true;
            }, "because we want to test the reason");

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                TestEnum = TestEnum.Type1
            }, assertion: async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.Should().Be(TestEnum.Type1);
                    completed = true;
                }, "we want to test the {0}", "reason");

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : 2 }""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                TestEnum = default(TestEnum)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.TestEnum.ShouldBe(TestEnum.Type1);
                completed = true;
            });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                TestEnum = default(TestEnum)
            }, assertion: async model =>
                {
                    await Task.Delay(10);
                    model.TestEnum.Should().Be(TestEnum.Type1);
                    completed = true;
                });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_with_no_values_in_range_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : -1 }""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                TestEnum = default(TestEnum)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.TestEnum.ShouldBe((TestEnum)(-1));
                completed = true;
            });

        // Assert
        act.ShouldNotThrow();
        completed.ShouldBeTrue();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                TestEnum = default(TestEnum)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.TestEnum.Should().Be((TestEnum)(-1));
                completed = true;
            });

        // Assert
        act.Should().NotThrow();
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_content_with_enums_without_having_satisfiable_assertion_to_satisfy_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "testEnum" : -1 }""", Encoding.UTF8, "application/json")
        };
        bool completed = false;

#if SH
        // Act
        Action act = () =>
            subject.ShouldSatisfy(givenModelStructure: new
            {
                TestEnum = default(TestEnum)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.TestEnum.ShouldBe(TestEnum.Type1);
                completed = true;
            }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should satisfy all the conditions specified, but does not.", "Error 1", "model.TestEnum", "should be", "TestEnum.Type1", "but was", "TestEnum.-1", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
        // Shouldly aborts the lambda on the first failing assertion, so the flag was never set.
        completed.ShouldBeFalse();
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                TestEnum = default(TestEnum)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.TestEnum.Should().Be(TestEnum.Type1);
                completed = true;
            }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*the enum to be TestEnum.Type1*, but found TestEnum.-1**HTTP response*");
        completed.Should().BeTrue();
#endif
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
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
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.ShouldBeNull();
            }, "because we want to test the reason");

        // Assert
        act.ShouldFailContaining("subject.Content", "should be as", "Exception while deserializing the model with SystemTextJsonSerializer", "Additional Info:", "because we want to test the reason", "The HTTP response was:");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.Should().BeNull();
            }, "we want to test the {0}", "reason");

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
            subject.ShouldSatisfy(givenModelStructure: (TestModel)null!, assertion: (Func<TestModel, Task>)null!);

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
            .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
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
            }, assertion: async model => await Task.Run(() => true.ShouldBeTrue()), "because we want to test the failure message");

        // Assert
        act.ShouldFailWith("subject", "should not be null", "Additional Info:", "because we want to test the failure message");
#else
        // Act
        Action act = () =>
            subject.Should().Satisfy(givenModelStructure: new
            {
                Property = default(string)
            }, assertion: async model => await Task.Run(() => true.Should().BeTrue()), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected a * to assert because we want to test the failure message, but found <null>.");
#endif
    }
    #endregion
}