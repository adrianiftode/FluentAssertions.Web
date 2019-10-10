using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests
{
    public class StatusesSpecs
    {
        #region 200 Ok
        [Fact]
        public void When_asserting_200_Ok_response_to_be_200_Ok_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be200Ok();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_200_Ok_response_to_be_200_Ok_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            Action act = () =>
                subject.Should().Be200Ok("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*OK because we want to test the failure message, but found BadRequest*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_200_Ok_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().Be200Ok("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 400 BadRequest
        [Fact]
        public void When_asserting_400_BadRequest_response_to_be_400_BadRequest_it_should_succeed()
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
        public void When_asserting_other_than_400_BadRequest_response_to_be_400_BadRequest_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*BadRequest because we want to test the failure message, but found OK*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_400_BadRequest_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 401 Unauthorized
        [Fact]
        public void When_asserting_401_Unauthorized_response_to_be_401_Unauthorized_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            // Act
            Action act = () =>
                subject.Should().Be401Unauthorized();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_401_Unauthorized_response_to_be_401_Unauthorized_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be401Unauthorized("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Unauthorized because we want to test the failure message, but found OK*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_401_Unauthorized_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().Be401Unauthorized("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 405 Method Not Allowed
        [Fact]
        public void When_asserting_405_MethodNotAllowed_response_to_be_405_MethodNotAllowed_it_should_succeed()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);

            // Act
            Action act = () =>
                subject.Should().Be405MethodNotAllowed();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_405_MethodNotAllowed_response_to_be_405_MethodNotAllowed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be405MethodNotAllowed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*MethodNotAllowed because we want to test the failure message, but found OK*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_405_MethodNotAllowed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().Be405MethodNotAllowed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion
    }
}