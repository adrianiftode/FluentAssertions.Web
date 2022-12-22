using FluentAssertions.Web.Tests.TestModels;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests;

public class SatisfyModelAssertionsAsyncSpecs
{
    #region Typed Model

    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertions_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };

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
            .WithMessage("Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*to be empty, but found \"Value\"*HTTP response*");
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_strings_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : \"Type1\"}", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : 2 }", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_with_no_values_in_range_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : -1 }", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_with_enums_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : -1 }", Encoding.UTF8, "application/json")
        };

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
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertions_it_should_throw_with_descriptive_message()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("\"True\"", Encoding.UTF8, "application/json")
        };

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
            subject.Should().Satisfy<TestModel>(
                async model =>
                {
                    await Task.Delay(10);
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
            subject.Should().Satisfy<TestModel>((Func<TestModel, Task>)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
    }

    [Fact]
    public void When_asserting_null_response_content_to_be_satisfy_it_should_throw_with_descriptive_message()
    {
        // Arrange
        HttpResponseMessage? subject = null;

        // Act
        Action act = () =>
            subject.Should().Satisfy<TestModel>(async model => await Task.Run(() => true.Should().BeTrue()), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected a * to assert because we want to test the failure message, but found <null>.");
    }
    #endregion

    #region Inferred Model
    [Fact]
    public void When_asserting_response_content_with_a_certain_assertion_to_satisfy_assertions_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };
        var completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_to_satisfy_against_null_given_model_type_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"property\" : \"Value\"}", Encoding.UTF8, "application/json")
        };
        var completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_without_having_satisfiable_assertion_to_satisfy_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
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
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.Should().BeEmpty();
            }, "we want to test the {0}", "reason");

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
            }, assertion: async model =>
                  {
                      await Task.Delay(10);
                      model.Property.Should().Be("Not Value");
                      model.Should().BeNull();
                  }, "we want to test the {0}", "reason");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected * to satisfy one or more model assertions, but it wasn't because we want to test the reason:*expected*Not Value*expected*to be <null>*The HTTP response was:*");
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_strings_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : \"Type1\"}", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : 2 }", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_with_enums_serialized_as_integers_with_no_values_in_range_inferred_from_model_it_should_succeed()
    {
        // Arrange
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : -1 }", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_content_with_enums_without_having_satisfiable_assertion_to_satisfy_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
    {
        using var subject = new HttpResponseMessage
        {
            Content = new StringContent("{ \"testEnum\" : -1 }", Encoding.UTF8, "application/json")
        };
        bool completed = false;

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
    }

    [Fact]
    public void When_asserting_response_with_not_a_proper_JSON_to_satisfy_assertions_inferred_from_model_it_should_throw_with_descriptive_message()
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
            }, assertion: async model =>
            {
                await Task.Delay(10);
                model.Property.Should().BeNull();
            }, "we want to test the {0}", "reason");

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
            .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
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
            }, assertion: async model => await Task.Run(() => true.Should().BeTrue()), "because we want to test the failure {0}", "message");

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(@"Expected a * to assert because we want to test the failure message, but found <null>.");
    }
    #endregion
}