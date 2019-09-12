using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            Func<Task> act = () =>
                subject.Should().BeBadRequest();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_ok_response_to_be_bad_request_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Func<Task> act = () =>
                subject.Should().BeBadRequest("because we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected BadRequest, but found OK because we want to test the failure message. The content was <null>.");
        }

        [Fact]
        public void When_asserting_bad_request_response_with_content_to_be_bad_request_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
    ""id"": [""Error message.""]   
}", Encoding.UTF8, "application/json")
            };

            // Act
            Func<Task> act = () =>
                subject.Should().BeBadRequest();

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
    ""id"": [""Error message.""]   
}", Encoding.UTF8, "application/json")
            };

            // Act
            Func<Task> act = async () =>
                (await subject.Should().BeBadRequest())
                    .WithError("id", "Error message.");

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
            Func<Task> act = async () =>
               (await response.Should().BeBadRequest())
                    .WithError("id", "Error message 1.")
                    .And
                    .WithError("id", "Error message 2.");

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
            Func<Task> act = async () =>
                (await subject.Should().BeBadRequest())
                .WithHttpHeader("Location", "www.go.to");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_no_headers_to_be_bad_request_and_have_header_and_header_value_it_should_throw()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            Func<Task> act = async () =>
                (await subject.Should().BeBadRequest())
                    .WithHttpHeader("Location", "www.go.to");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected value to contain the HttpHeader \"Location\" with content \"www.go.to\", but no such header was found in the actual headers list: {empty}. The response content was <null>");
        }
    }
}

