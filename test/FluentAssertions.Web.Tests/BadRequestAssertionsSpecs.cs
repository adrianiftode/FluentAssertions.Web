using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests
{
    public class BadRequestAssertionsSpecs
    {
        [Fact]
        public void When_asserting_bad_request_response_to_be_bad_request_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_ok_response_to_be_bad_request_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*BadRequest because we want to test the failure message, but found OK*");
        }

        [Fact]
        public void When_asserting_bad_request_response_with_content_to_be_bad_request_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
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
        public void When_asserting_bad_request_response_with_standard_net_json_content_to_be_bad_request_with_error_field_and_error_message_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
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
                .And.WithError("Author", "The Author field is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_multiple_error_messages_to_be_bad_request_with_error_field_and_error_message_and_also_having_another_error_message_it_should_succeed()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                        ""id"": [""Error message 1."", ""Error message 2.""]   
                    }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () =>
               response.Should().Be400BadRequest()
                    .And.WithError("id", "Error message 1.")
                    .And.WithError("id", "Error message 2.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_header_and_header_value_to_be_bad_request_and_have_header_and_header_value_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Headers =
                {
                    { "Location", "www.go.to" }
                }
            };

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest()
                    .And.WithHttpHeader("Location", "www.go.to");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_no_headers_to_be_bad_request_and_have_header_and_header_value_it_should_throw()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest()
                    .And.WithHttpHeader("Location", "www.go.to");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Location*www.go.to*");
        }

        [Fact]
        public void When_asserting_bad_request_response_with_only_the_error_description_and_no_name_for_the_error_field_to_be_bad_request_with_specific_message_it_should_succeed()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
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
                    .And.WithError("", "A non-empty request body is required.");

            // Assert
            act.Should().NotThrow();
        }
    }
}

