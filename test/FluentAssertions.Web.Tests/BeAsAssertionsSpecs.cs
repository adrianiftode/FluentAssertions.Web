using FluentAssertions.Web.Internal;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests
{
    public class BeAsAssertionsSpecs
    {
        [Fact]
        public void When_asserting_response_with_content_to_be_as_model_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage
            {
                Content = new StringContent(@"{
                                            ""comment"": ""Hey"",
                                            ""author"": ""John"",
                                        }", Encoding.UTF8, "application/json")
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
                Content = new StringContent(@"{
                                            ""author"": ""John"",
                                            ""comment"": ""Hey"",
                                        }", Encoding.UTF8, "application/json")
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
                Content = new StringContent(@"{
                                            ""comment"": ""Hey"",
                                            ""author"": ""John"",
                                        }", Encoding.UTF8, "application/json")
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
        public void When_asserting_response_with_fewer_JSON_properties_than_the_model_to_be_as_model_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage
            {
                Content = new StringContent(@"{
                                            ""comment"": ""Hey"",
                                        }", Encoding.UTF8, "application/json")
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
                .WithMessage(@"*Author to be ""John""*reason*");
        }

        [Fact]
        public void When_asserting_response_with_not_a_proper_JSON_to_be_as_model_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage
            {
                Content = new StringContent(@"some text:that doesn't look like a json {", Encoding.UTF8, "text/plain")
            };

            // Act
            Action act = () =>
                subject.Should().BeAs(new
                {
                    Comment = "Hey"
                }, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*to have a content equivalent to a model, but the JSON respresentation could not be parsed*");
        }

        [Fact]
        public void When_asserting_response_to_be_as_against_null_value_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().BeAs((object)null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*content*<null>*");
        }

        [Fact]
        public void When_asserting_null_response_to_be_as_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().BeAs((object)null, "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
    }
}
