using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace FluentAssertions.Web.Tests
{
    public class BadRequestAssertionsSpecs
    {
        [Fact]
        public void When_asserting_bad_request_response_with_content_to_be_BadRequest_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                                            ""errors"": {
                                                ""Author"": [
                                                    ""The Author field is required.""
                                                ]
                                            }
                                        }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_standard_net_json_content_to_be_BadRequest_and_have_error_field_and_error_message_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Author", "The Author field is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_be_BadRequest_and_match_error_message_by_pattern_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Author", "*required*");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_multiple_error_messages_to_be_BadRequest_and_have_error_field_and_error_message_and_also_having_another_error_message_it_should_succeed()
        {
            // Arrange
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                        ""id"": [""Error message 1."", ""Error message 2.""]   
                    }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () =>
               response.Should().Be400BadRequest()
                    .And.HaveError("id", "Error message 1.")
                    .And.HaveError("id", "Error message 2.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_only_the_error_description_and_no_name_for_the_error_field_to_be_BadRequest_with_specific_message_it_should_succeed()
        {
            // Arrange
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{ 
                           ""errors"":{ 
                              """":[ 
                                 ""A non-empty request body is required.""
                              ]
                           }
                        }", Encoding.UTF8, "application/json")
            };

            // Act 
            Action act = () =>
                response.Should().Be400BadRequest()
                    .And.HaveErrorMessage("A non-empty request body is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_asserting_bad_request_response_to_be_BadRequest_And_HaveError_against_null_or_empty_string_value_for_the_error_field_it_should_throw_with_descriptive_message(string expectedField)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError(expectedField, "*required*");

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("*<null> or empty field name*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_asserting_bad_request_response_to_be_BadRequest_And_HaveError_against_null_or_empty_string_value_for_the_error_message_it_should_throw_with_descriptive_message(string expectedWildcardErrorMessage)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Author", expectedWildcardErrorMessage);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("*<null> or empty wildcard error message*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_asserting_bad_request_response_to_be_BadRequest_And_HaveErrorMessage_against_null_or_empty_string_value_for_the_error_message_it_should_throw_with_descriptive_message(string expectedWildcardErrorMessage)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveErrorMessage(expectedWildcardErrorMessage);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("*<null> or empty wildcard error message*");
        }
    }
}

