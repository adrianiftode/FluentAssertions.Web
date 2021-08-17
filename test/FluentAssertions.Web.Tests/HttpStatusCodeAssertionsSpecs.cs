using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests
{
    public class HttpStatusCodeAssertionsSpecs
    {
        #region Be1XXInformational
        [Theory]
        [InlineData(HttpStatusCode.Continue)]
        [InlineData(HttpStatusCode.SwitchingProtocols)]
        public void When_asserting_response_with_an_informational_status_to_Be1XXInformational_it_should_succeed(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be1XXInformational();

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void When_asserting_other_than_an_informational_status_code_response_to_Be1XXInformational_it_should_throw_with_descriptive_message(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be1XXInformational("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*to have a HTTP status code representing an informational error*message*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_Be1XXInformational_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be1XXInformational("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region Be2XXSuccessful
        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.PartialContent)]
        public void When_asserting_response_with_a_successful_status_to_Be2XXSuccessful_it_should_succeed(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be2XXSuccessful();

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(HttpStatusCode.SwitchingProtocols)]
        [InlineData(HttpStatusCode.PermanentRedirect)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void When_asserting_other_than_a_successful_status_code_response_to_Be2XXSuccessful_it_should_throw_with_descriptive_message(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be2XXSuccessful("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*have a successful HTTP status code, but it was*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_Be2XXSuccessful_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be2XXSuccessful("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region Be3XXRedirection
        [Theory]
        [InlineData(HttpStatusCode.Moved)]
        [InlineData(HttpStatusCode.TemporaryRedirect)]
        public void When_asserting_response_with_a_redirection_status_code_to_Be3XXRedirection_it_should_succeed(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be3XXRedirection();

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(HttpStatusCode.Continue)]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void When_asserting_other_than_a_client_redirection_status_code_response_to_Be3XXRedirection_it_should_throw_with_descriptive_message(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be3XXRedirection("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*to have a HTTP status code representing a redirection*message*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_Be3XXRedirection_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be3XXRedirection("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region Be4XXClientError
        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.UpgradeRequired)]
        public void When_asserting_response_with_a_client_error_status_code_to_Be4XXClientError_it_should_succeed(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be4XXClientError();

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(HttpStatusCode.Continue)]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public void When_asserting_other_than_a_client_error_status_code_response_to_Be4XXClientError_it_should_throw_with_descriptive_message(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be4XXClientError("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*to have a HTTP status code representing a client error*message*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_Be4XXClientError_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be4XXClientError("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region Be5XXServerError
        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.HttpVersionNotSupported)]
        public void When_asserting_response_with_a_server_error_status_code_to_Be5XXServerError_it_should_succeed(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be5XXServerError();

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(HttpStatusCode.Continue)]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.UpgradeRequired)]
        public void When_asserting_other_than_a_server_error_status_code_response_to_Be5XXServerError_it_should_throw_with_descriptive_message(HttpStatusCode status)
        {
            // Arrange
            using var subject = new HttpResponseMessage(status);

            // Act
            Action act = () =>
                subject.Should().Be5XXServerError("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*to have a HTTP status code representing a server error*message*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_Be5XXServerError_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be5XXServerError("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region HaveHttpStatusCode
        [Fact]
        public void When_asserting_response_with_a_status_to_HaveHttpStatusCode_with_that_status_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Continue);

            // Act
            Action act = () =>
                subject.Should().HaveHttpStatusCode(HttpStatusCode.Continue);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_the_expected_status_response_to_HaveHttpStatusCode_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().HaveHttpStatusCode(HttpStatusCode.BadRequest, "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*BadRequest"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_HaveHttpStatusCode_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().HaveHttpStatusCode(HttpStatusCode.InternalServerError, "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region NotHaveHttpStatusCode
        [Fact]
        public void When_asserting_response_with_a_status_to_NotHaveHttpStatusCode_with_that_other_status_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Continue);

            // Act
            Action act = () =>
                subject.Should().NotHaveHttpStatusCode(HttpStatusCode.InsufficientStorage);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_response_with_the_unexpected_status_to_NotHaveHttpStatusCode_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().NotHaveHttpStatusCode(HttpStatusCode.OK, "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Did not expect *to have status HttpStatusCode.OK {value: 200}*message*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_NotHaveHttpStatusCode_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().NotHaveHttpStatusCode(HttpStatusCode.InternalServerError, "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 100 Continue
        [Fact]
        public void When_asserting_100_Continue_response_to_be_100Continue_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Continue);

            // Act
            Action act = () =>
                subject.Should().Be100Continue();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_100_Continue_response_to_be_100Continue_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be100Continue("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Continue"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_100Continue_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be100Continue("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 101 Switching Protocols
        [Fact]
        public void When_asserting_101_Switching_Protocols_response_to_be_101SwitchingProtocols_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);

            // Act
            Action act = () =>
                subject.Should().Be101SwitchingProtocols();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_101_Switching_Protocols_response_to_be_101SwitchingProtocols_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be101SwitchingProtocols("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*SwitchingProtocols"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_101SwitchingProtocols_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be101SwitchingProtocols("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion 

        #region 200 Ok
        [Fact]
        public void When_asserting_200_Ok_response_to_be_200_Ok_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

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
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            Action act = () =>
                subject.Should().Be200Ok("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*OK"" because we want to test the failure message, but found HttpStatusCode.BadRequest {value: 400}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_200_Ok_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be200Ok("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 201 Created
        [Fact]
        public void When_asserting_201_Created_response_to_be_201Created_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Created);

            // Act
            Action act = () =>
                subject.Should().Be201Created();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_201_Created_response_to_be_201Created_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be201Created("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Created"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_201Created_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be201Created("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 202 Accepted
        [Fact]
        public void When_asserting_202_Accepted_response_to_be_202Accepted_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Accepted);

            // Act
            Action act = () =>
                subject.Should().Be202Accepted();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_202_Accepted_response_to_be_202Accepted_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be202Accepted("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Accepted"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_202Accepted_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be202Accepted("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 203 Non Authoritative Information
        [Fact]
        public void When_asserting_203_Non_Authoritative_Information_response_to_be_203NonAuthoritativeInformation_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);

            // Act
            Action act = () =>
                subject.Should().Be203NonAuthoritativeInformation();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_203_Non_Authoritative_Information_response_to_be_203NonAuthoritativeInformation_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be203NonAuthoritativeInformation("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*NonAuthoritativeInformation"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_203NonAuthoritativeInformation_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be203NonAuthoritativeInformation("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 204 No Content
        [Fact]
        public void When_asserting_204_No_Content_response_to_be_204NoContent_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.NoContent);

            // Act
            Action act = () =>
                subject.Should().Be204NoContent();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_204_No_Content_response_to_be_204NoContent_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be204NoContent("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*NoContent"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_204NoContent_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be204NoContent("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 205 Reset Content
        [Fact]
        public void When_asserting_205_Reset_Content_response_to_be_205ResetContent_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.ResetContent);

            // Act
            Action act = () =>
                subject.Should().Be205ResetContent();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_205_Reset_Content_response_to_be_205ResetContent_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be205ResetContent("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*ResetContent"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_205ResetContent_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be205ResetContent("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 206 Partial Content
        [Fact]
        public void When_asserting_206_Partial_Content_response_to_be_206PartialContent_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.PartialContent);

            // Act
            Action act = () =>
                subject.Should().Be206PartialContent();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_206_Partial_Content_response_to_be_206PartialContent_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be206PartialContent("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*PartialContent"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_206PartialContent_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be206PartialContent("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 300 Multiple Choices
        [Fact]
        public void When_asserting_300_Multiple_Choices_response_to_be_300MultipleChoices_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.MultipleChoices);

            // Act
            Action act = () =>
                subject.Should().Be300MultipleChoices();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_300_Multiple_Choices_response_to_be_300MultipleChoices_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be300MultipleChoices("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*MultipleChoices"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_300MultipleChoices_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be300MultipleChoices("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 300 Ambiguous
        [Fact]
        public void When_asserting_300_Ambiguous_response_to_be_300Ambiguous_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Ambiguous);

            // Act
            Action act = () =>
                subject.Should().Be300Ambiguous();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_300_Ambiguous_response_to_be_300Ambiguous_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be300Ambiguous("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Ambiguous"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_300Ambiguous_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be300Ambiguous("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 301 Moved Permanently
        [Fact]
        public void When_asserting_301_Moved_Permanently_response_to_be_301MovedPermanently_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.MovedPermanently);

            // Act
            Action act = () =>
                subject.Should().Be301MovedPermanently();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_301_Moved_Permanently_response_to_be_301MovedPermanently_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be301MovedPermanently("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*MovedPermanently"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_301MovedPermanently_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be301MovedPermanently("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 301 Moved
        [Fact]
        public void When_asserting_301_Moved_response_to_be_301Moved_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Moved);

            // Act
            Action act = () =>
                subject.Should().Be301Moved();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_301_Moved_response_to_be_301Moved_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be301Moved("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Moved"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_301Moved_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be301Moved("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 302 Found
        [Fact]
        public void When_asserting_302_Found_response_to_be_302Found_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Found);

            // Act
            Action act = () =>
                subject.Should().Be302Found();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_302_Found_response_to_be_302Found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be302Found("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Found"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_302Found_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be302Found("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 302 Redirect
        [Fact]
        public void When_asserting_302_Redirect_response_to_be_302Redirect_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Redirect);

            // Act
            Action act = () =>
                subject.Should().Be302Redirect();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_302_Redirect_response_to_be_302Redirect_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be302Redirect("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Redirect"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_302Redirect_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be302Redirect("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 303 See Other
        [Fact]
        public void When_asserting_303_See_Other_response_to_be_303SeeOther_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.SeeOther);

            // Act
            Action act = () =>
                subject.Should().Be303SeeOther();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_303_See_Other_response_to_be_303SeeOther_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be303SeeOther("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*SeeOther"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_303SeeOther_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be303SeeOther("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 303 Redirect Method
        [Fact]
        public void When_asserting_303_Redirect_Method_response_to_be_303RedirectMethod_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.RedirectMethod);

            // Act
            Action act = () =>
                subject.Should().Be303RedirectMethod();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_303_Redirect_Method_response_to_be_303RedirectMethod_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be303RedirectMethod("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*RedirectMethod"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_303RedirectMethod_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be303RedirectMethod("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 304 Not Modified
        [Fact]
        public void When_asserting_304_Not_Modified_response_to_be_304NotModified_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.NotModified);

            // Act
            Action act = () =>
                subject.Should().Be304NotModified();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_304_Not_Modified_response_to_be_304NotModified_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be304NotModified("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*NotModified"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_304NotModified_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be304NotModified("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 305 Use Proxy
        [Fact]
        public void When_asserting_305_Use_Proxy_response_to_be_305UseProxy_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.UseProxy);

            // Act
            Action act = () =>
                subject.Should().Be305UseProxy();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_305_Use_Proxy_response_to_be_305UseProxy_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be305UseProxy("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*UseProxy"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_305UseProxy_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be305UseProxy("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 306 Unused
        [Fact]
        public void When_asserting_306_Unused_response_to_be_306Unused_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Unused);

            // Act
            Action act = () =>
                subject.Should().Be306Unused();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_306_Unused_response_to_be_306Unused_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be306Unused("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Unused"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_306Unused_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be306Unused("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 307 Temporary Redirect
        [Fact]
        public void When_asserting_307_Temporary_Redirect_response_to_be_307TemporaryRedirect_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.TemporaryRedirect);

            // Act
            Action act = () =>
                subject.Should().Be307TemporaryRedirect();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_307_Temporary_Redirect_response_to_be_307TemporaryRedirect_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be307TemporaryRedirect("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*TemporaryRedirect"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_307TemporaryRedirect_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be307TemporaryRedirect("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 307 Redirect Keep Verb
        [Fact]
        public void When_asserting_307_Redirect_Keep_Verb_response_to_be_307RedirectKeepVerb_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.RedirectKeepVerb);

            // Act
            Action act = () =>
                subject.Should().Be307RedirectKeepVerb();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_307_Redirect_Keep_Verb_response_to_be_307RedirectKeepVerb_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be307RedirectKeepVerb("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*RedirectKeepVerb"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_307RedirectKeepVerb_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be307RedirectKeepVerb("because we want to test the failure {0}", "message"); ;

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
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest);

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
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be400BadRequest("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*BadRequest"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_400_BadRequest_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

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
            using var subject = new HttpResponseMessage(HttpStatusCode.Unauthorized);

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
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be401Unauthorized("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Unauthorized"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_401_Unauthorized_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be401Unauthorized("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 402 Payment Required
        [Fact]
        public void When_asserting_402_Payment_Required_response_to_be_402PaymentRequired_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.PaymentRequired);

            // Act
            Action act = () =>
                subject.Should().Be402PaymentRequired();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_402_Payment_Required_response_to_be_402PaymentRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be402PaymentRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*PaymentRequired"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_402PaymentRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be402PaymentRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 403 Forbidden
        [Fact]
        public void When_asserting_403_Forbidden_response_to_be_403Forbidden_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Forbidden);

            // Act
            Action act = () =>
                subject.Should().Be403Forbidden();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_403_Forbidden_response_to_be_403Forbidden_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be403Forbidden("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Forbidden"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_403Forbidden_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be403Forbidden("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 404 Not Found
        [Fact]
        public void When_asserting_404_Not_Found_response_to_be_404NotFound_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.NotFound);

            // Act
            Action act = () =>
                subject.Should().Be404NotFound();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_404_Not_Found_response_to_be_404NotFound_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be404NotFound("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*NotFound"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_404NotFound_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be404NotFound("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 405 Method Not Allowed
        [Fact]
        public void When_asserting_405_Method_Not_Allowed_response_to_be_405MethodNotAllowed_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);

            // Act
            Action act = () =>
                subject.Should().Be405MethodNotAllowed();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_405_Method_Not_Allowed_response_to_be_405MethodNotAllowed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be405MethodNotAllowed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*MethodNotAllowed"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_405MethodNotAllowed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be405MethodNotAllowed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 406 Not Acceptable
        [Fact]
        public void When_asserting_406_Not_Acceptable_response_to_be_406NotAcceptable_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.NotAcceptable);

            // Act
            Action act = () =>
                subject.Should().Be406NotAcceptable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_406_Not_Acceptable_response_to_be_406NotAcceptable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be406NotAcceptable("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*NotAcceptable"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_406NotAcceptable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be406NotAcceptable("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 407 Proxy Authentication Required
        [Fact]
        public void When_asserting_407_Proxy_Authentication_Required_response_to_be_407ProxyAuthenticationRequired_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.ProxyAuthenticationRequired);

            // Act
            Action act = () =>
                subject.Should().Be407ProxyAuthenticationRequired();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_407_Proxy_Authentication_Required_response_to_be_407ProxyAuthenticationRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be407ProxyAuthenticationRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*ProxyAuthenticationRequired"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_407ProxyAuthenticationRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be407ProxyAuthenticationRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 408 Request Timeout
        [Fact]
        public void When_asserting_408_Request_Timeout_response_to_be_408RequestTimeout_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

            // Act
            Action act = () =>
                subject.Should().Be408RequestTimeout();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_408_Request_Timeout_response_to_be_408RequestTimeout_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be408RequestTimeout("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*RequestTimeout"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_408RequestTimeout_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be408RequestTimeout("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 409 Conflict
        [Fact]
        public void When_asserting_409_Conflict_response_to_be_409Conflict_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Conflict);

            // Act
            Action act = () =>
                subject.Should().Be409Conflict();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_409_Conflict_response_to_be_409Conflict_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be409Conflict("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Conflict"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_409Conflict_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be409Conflict("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 410 Gone
        [Fact]
        public void When_asserting_410_Gone_response_to_be_410Gone_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.Gone);

            // Act
            Action act = () =>
                subject.Should().Be410Gone();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_410_Gone_response_to_be_410Gone_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be410Gone("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Gone"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_410Gone_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be410Gone("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 411 Length Required
        [Fact]
        public void When_asserting_411_Length_Required_response_to_be_411LengthRequired_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.LengthRequired);

            // Act
            Action act = () =>
                subject.Should().Be411LengthRequired();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_411_Length_Required_response_to_be_411LengthRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be411LengthRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*LengthRequired"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_411LengthRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be411LengthRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 412 Precondition Failed
        [Fact]
        public void When_asserting_412_Precondition_Failed_response_to_be_412PreconditionFailed_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.PreconditionFailed);

            // Act
            Action act = () =>
                subject.Should().Be412PreconditionFailed();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_412_Precondition_Failed_response_to_be_412PreconditionFailed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be412PreconditionFailed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*PreconditionFailed"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_412PreconditionFailed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be412PreconditionFailed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 413 Request Entity Too Large
        [Fact]
        public void When_asserting_413_Request_Entity_Too_Large_response_to_be_413RequestEntityTooLarge_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);

            // Act
            Action act = () =>
                subject.Should().Be413RequestEntityTooLarge();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_413_Request_Entity_Too_Large_response_to_be_413RequestEntityTooLarge_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be413RequestEntityTooLarge("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*RequestEntityTooLarge"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_413RequestEntityTooLarge_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be413RequestEntityTooLarge("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 414 Request Uri Too Long
        [Fact]
        public void When_asserting_414_Request_Uri_Too_Long_response_to_be_414RequestUriTooLong_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.RequestUriTooLong);

            // Act
            Action act = () =>
                subject.Should().Be414RequestUriTooLong();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_414_Request_Uri_Too_Long_response_to_be_414RequestUriTooLong_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be414RequestUriTooLong("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*RequestUriTooLong"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_414RequestUriTooLong_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be414RequestUriTooLong("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 415 Unsupported Media Type
        [Fact]
        public void When_asserting_415_Unsupported_Media_Type_response_to_be_415UnsupportedMediaType_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

            // Act
            Action act = () =>
                subject.Should().Be415UnsupportedMediaType();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_415_Unsupported_Media_Type_response_to_be_415UnsupportedMediaType_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be415UnsupportedMediaType("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*UnsupportedMediaType"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_415UnsupportedMediaType_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be415UnsupportedMediaType("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 416 Requested Range Not Satisfiable
        [Fact]
        public void When_asserting_416_Requested_Range_Not_Satisfiable_response_to_be_416RequestedRangeNotSatisfiable_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);

            // Act
            Action act = () =>
                subject.Should().Be416RequestedRangeNotSatisfiable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_416_Requested_Range_Not_Satisfiable_response_to_be_416RequestedRangeNotSatisfiable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be416RequestedRangeNotSatisfiable("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*RequestedRangeNotSatisfiable"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_416RequestedRangeNotSatisfiable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be416RequestedRangeNotSatisfiable("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 417 Expectation Failed
        [Fact]
        public void When_asserting_417_Expectation_Failed_response_to_be_417ExpectationFailed_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            // Act
            Action act = () =>
                subject.Should().Be417ExpectationFailed();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_417_Expectation_Failed_response_to_be_417ExpectationFailed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be417ExpectationFailed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*ExpectationFailed"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_417ExpectationFailed_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be417ExpectationFailed("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 426 Upgrade Required
        [Fact]
        public void When_asserting_426_Upgrade_Required_response_to_be_426UpgradeRequired_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.UpgradeRequired);

            // Act
            Action act = () =>
                subject.Should().Be426UpgradeRequired();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_426_Upgrade_Required_response_to_be_426UpgradeRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be426UpgradeRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*UpgradeRequired"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_426UpgradeRequired_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be426UpgradeRequired("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 500 Internal Server Error
        [Fact]
        public void When_asserting_500_Internal_Server_Error_response_to_be_500InternalServerError_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            // Act
            Action act = () =>
                subject.Should().Be500InternalServerError();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_500_Internal_Server_Error_response_to_be_500InternalServerError_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be500InternalServerError("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*InternalServerError"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_500InternalServerError_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be500InternalServerError("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 501 Not Implemented
        [Fact]
        public void When_asserting_501_Not_Implemented_response_to_be_501NotImplemented_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.NotImplemented);

            // Act
            Action act = () =>
                subject.Should().Be501NotImplemented();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_501_Not_Implemented_response_to_be_501NotImplemented_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be501NotImplemented("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*NotImplemented"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_501NotImplemented_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be501NotImplemented("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 502 Bad Gateway
        [Fact]
        public void When_asserting_502_Bad_Gateway_response_to_be_502BadGateway_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadGateway);

            // Act
            Action act = () =>
                subject.Should().Be502BadGateway();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_502_Bad_Gateway_response_to_be_502BadGateway_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be502BadGateway("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*BadGateway"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_502BadGateway_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be502BadGateway("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 503 Service Unavailable
        [Fact]
        public void When_asserting_503_Service_Unavailable_response_to_be_503ServiceUnavailable_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

            // Act
            Action act = () =>
                subject.Should().Be503ServiceUnavailable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_503_Service_Unavailable_response_to_be_503ServiceUnavailable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be503ServiceUnavailable("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*ServiceUnavailable"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_503ServiceUnavailable_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be503ServiceUnavailable("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 504 Gateway Timeout
        [Fact]
        public void When_asserting_504_Gateway_Timeout_response_to_be_504GatewayTimeout_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);

            // Act
            Action act = () =>
                subject.Should().Be504GatewayTimeout();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_504_Gateway_Timeout_response_to_be_504GatewayTimeout_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be504GatewayTimeout("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*GatewayTimeout"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_504GatewayTimeout_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be504GatewayTimeout("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion

        #region 505 Http Version Not Supported
        [Fact]
        public void When_asserting_505_Http_Version_Not_Supported_response_to_be_505HttpVersionNotSupported_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.HttpVersionNotSupported);

            // Act
            Action act = () =>
                subject.Should().Be505HttpVersionNotSupported();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_other_than_505_Http_Version_Not_Supported_response_to_be_505HttpVersionNotSupported_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK);

            // Act
            Action act = () =>
                subject.Should().Be505HttpVersionNotSupported("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*HttpVersionNotSupported"" because we want to test the failure message, but found HttpStatusCode.OK {value: 200}*");
        }

        [Fact]
        public void When_asserting_null_HttpResponse_to_be_505HttpVersionNotSupported_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage? subject = null;

            // Act
            Action act = () =>
                subject.Should().Be505HttpVersionNotSupported("because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
        #endregion 
    }
}