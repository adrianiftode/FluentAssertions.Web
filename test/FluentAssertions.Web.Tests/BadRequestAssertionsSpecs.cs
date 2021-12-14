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

        #region HaveError
        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }")]

        [InlineData(@"{
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                }")]
        public void When_asserting_bad_request_response_with_standard_dot_net_json_content_to_be_BadRequest_and_have_error_field_and_error_message_it_should_succeed(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Author", "The Author field is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_error_field_having_a_reserved_name_from_standard_dot_net_json_to_be_BadRequest_and_HaveError_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                  ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
                  ""title"": ""One or more validation errors occurred."",
                  ""status"": 400,
                  ""traceId"": ""00-deb7480af23a884e942f7b85cac6bd35-c1abe72874d40c4a-00"",
                  ""errors"": {
                    ""Status"": [
                      ""Cannot add at Revoked Status.""
                    ]
                  }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Status", "Cannot add at Revoked Status.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_error_field_having_a_reserved_name_from_standard_dot_net_json_and_also_error_message_to_be_BadRequest_and_HaveError_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                  ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
                  ""title"": ""One or more validation errors occurred."",
                  ""status"": 400,
                  ""traceId"": ""00-deb7480af23a884e942f7b85cac6bd35-c1abe72874d40c4a-00"",
                  ""errors"": {
                    ""Status"": [
                      ""2""
                    ]
                  }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("status", "2");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_error_field_having_a_reserved_name_from_standard_dot_net_json_which_does_not_exist_in_the_errors_property_to_be_BadRequest_and_HaveError_should_fail()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                  ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
                  ""title"": ""One or more validation errors occurred."",
                  ""status"": 400,
                  ""traceId"": ""00-deb7480af23a884e942f7b85cac6bd35-c1abe72874d40c4a-00"",
                  ""errors"": {
                  }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("status", "400");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to contain*status*field, but was not found*");
        }

        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }")]
        [InlineData(@"{
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                }")]
        public void When_asserting_bad_request_response_without_a_containing_error_to_be_BadRequest_and_HaveError_it_should_throw_with_descriptive_message(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Comments", "*required*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to contain*Comments*field, but was not found*");
        }

        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }")]

        [InlineData(@"{
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                }")]
        public void When_asserting_bad_request_response_be_BadRequest_and_match_error_message_by_pattern_it_should_succeed(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Author", "*required*");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(@"{
                        ""id"": [""Error message 1."", ""Error message 2.""]   
                    }")]
        [InlineData(@"{
                    ""errors"" :
                        {
                            ""id"": [""Error message 1."", ""Error message 2.""]   
                        }
        }")]
        public void When_asserting_bad_request_response_with_multiple_error_messages_to_be_BadRequest_and_have_error_field_and_error_message_and_also_having_another_error_message_it_should_succeed(string responseContent)
        {
            // Arrange
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () =>
               response.Should().Be400BadRequest()
                    .And.HaveError("id", "Error message 1.")
                    .And.HaveError("id", "Error message 2.");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(@"{
                        ""Author"": ""The Author field is required.""
                }")]

        [InlineData(@"{
                    ""errors"" : {
                        ""Author"": ""The Author field is required.""
                }
        }")]
        public void When_asserting_bad_request_response_with_the_errors_messages_as_a_single_field_to_be_BadRequest_and_have_error_field_and_error_message_it_should_succeed(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("Author", "The Author field is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(@"[
                        {
                            ""code"": ""previous_steps_validation_error"",
                            ""description"": null
                        }
                    ]")]

        [InlineData(@"{ ""errors"" : [
                        {
                            ""code"": ""previous_steps_validation_error"",
                            ""description"": null
                        }
                    ] }")]
        public void When_asserting_bad_request_response_with_a_response_content_having_an_array_to_BeBadRequest_and_have_error_field_and_error_message_it_should_throw_with_descriptive_message(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveError("error", "*required*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*error*");
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
        #endregion

        #region HaveErrorMessage
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

        [Fact]
        public void When_asserting_bad_request_response_with_the_errors_messages_as_a_single_field_to_be_BadRequest_and_HaveErrorMessage_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                        ""Author"": ""The Author field is required.""
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveErrorMessage("The Author field is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_a_list_errors_messages_to_be_BadRequest_and_HaveErrorMessage_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                        ""Author"": [ ""The Author field is required."" ]
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveErrorMessage("The Author field is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_content_generated_by_AspNetCore22_to_be_BadRequest_with_specific_message_it_should_succeed()
        {
            // Arrange
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{ 
                              """":[ 
                                 ""A non-empty request body is required.""
                              ]                           
                        }", Encoding.UTF8, "application/json")
            };

            // Act 
            Action act = () =>
                response.Should().Be400BadRequest()
                    .And.HaveErrorMessage("A non-empty request body is required.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_only_the_error_description_and_a_name_for_the_error_field_to_be_BadRequest_with_specific_message_it_should_succeed()
        {
            // Arrange
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                        ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
                        ""title"": ""One or more validation errors occurred."",
                        ""status"": 400,
                        ""traceId"": ""|2e128730-44b8587a25408f28."",
                        ""errors"": {
                                ""$"": [
                                    ""The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.""
                            ]
                    }
                }", Encoding.UTF8, "application/json")
            };

            // Act 
            Action act = () =>
                response.Should().Be400BadRequest()
                    .And.HaveErrorMessage("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_error_field_having_a_reserved_name_from_standard_dot_net_json_which_does_not_exist_in_the_errors_property_to_be_BadRequest_and_HaveErrorMessage_should_fail()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                  ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
                  ""title"": ""One or more validation errors occurred."",
                  ""status"": 400,
                  ""traceId"": ""00-deb7480af23a884e942f7b85cac6bd35-c1abe72874d40c4a-00"",
                  ""errors"": {
                  }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.HaveErrorMessage("One or more validation errors occurred.");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected response to contain the error message \"One or more validation errors occurred.\", but no such message was found in the actual error messages list*");
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
        #endregion

        #region NotHaveError
        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }")]
        [InlineData(@"{
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                }")]
        public void When_asserting_bad_request_response_with_standard_to_be_BadRequest_and_NotHaveError_it_should_succeed(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.NotHaveError("Comments");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_bad_request_response_with_error_field_having_a_reserved_name_from_standard_dot_net_json_which_does_not_exist_in_the_errors_property_to_be_BadRequest_and_NotHaveError_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                  ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
                  ""title"": ""One or more validation errors occurred."",
                  ""status"": 400,
                  ""traceId"": ""00-deb7480af23a884e942f7b85cac6bd35-c1abe72874d40c4a-00"",
                  ""errors"": {
                         ""Author"": [
                                            ""The Author field is required.""
                                     ]
                  }
                }", Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.NotHaveError("status");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                    }
                }")]

        [InlineData(@"{
                        ""Author"": [
                            ""The Author field is required.""
                        ]
                }")]
        public void When_asserting_bad_request_response_with_a_containing_error_to_be_BadRequest_and_NotHaveError_it_should_throw_with_descriptive_message(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.NotHaveError("Author");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to not contain*Author*field, but was found*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_asserting_bad_request_response_to_be_BadRequest_And_NotHaveError_against_null_or_empty_string_value_for_the_expected_error_field_it_should_throw_with_descriptive_message(string expectedErrorField)
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
                .And.NotHaveError(expectedErrorField);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*not having*<null> or empty field name*");
        }
        #endregion

        #region OnlyHaveError
        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [ ""The Author field is required."" ]
                    }
                }")]
        [InlineData(@"{
                    ""Author"": [ ""The Author field is required."" ]
                    }")]
        public void When_asserting_bad_request_response_with_a_single_error_field_to_be_BadRequest_and_OnlyHaveError_for_that_field_it_should_succeed(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.OnlyHaveError("Author", "*required*");

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [ ""The Author field is required."" ],
                        ""Comments"": [ ""The Comments field is required."" ]
                    }
                }")]
        [InlineData(@"{
                    ""Author"": [ ""The Author field is required."" ],
                    ""Comments"": [ ""The Comments field is required."" ]
                }")]
        public void When_asserting_bad_request_response_with_multiple_error_fields_where_one_of_them_is_the_actual_error_from_standard_dot_net_json_to_be_BadRequest_and_OnlyHaveError_fo_it_should_throw_with_descriptive_message(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.OnlyHaveError("Author", "*required*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*author*but more than this one was found.*");
        }

        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""Message 1."",
                            ""Message 2.""
                        ]
                    }
                }")]
        [InlineData(@"{
                        ""Author"": [
                            ""Message 1."",
                            ""Message 2.""
                        ]
                }")]
        public void When_asserting_bad_request_response_with_multiple_error_messages_where_one_of_them_is_the_actual_error_from_standard_dot_net_json_to_be_BadRequest_and_OnlyHaveError_fo_it_should_throw_with_descriptive_message(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.OnlyHaveError("Author", "*Message 1*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*author* field and message *Message 1*but more than this one was found.*");
        }

        [Theory]
        [InlineData(@"{
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ],
                        ""Comments"": [
                            ""The Comments field is required.""
                        ]
                    }
                }")]
        [InlineData(@"{
                        ""Author"": [
                            ""The Author field is required.""
                        ],
                        ""Comments"": [
                            ""The Comments field is required.""
                        ]
                }")]
        public void When_asserting_bad_request_response_with_multiple_error_fields_where_none_one_of_them_is_the_actual_error_to_be_BadRequest_and_OnlyHaveError_it_should_throw_with_descriptive_message(string responseContent)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            // Act
            Action act = () => subject.Should().Be400BadRequest()
                .And.OnlyHaveError("Date", "*required*");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to contain*Date*field, but was not found*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_asserting_bad_request_response_to_be_BadRequest_And_OnlyHaveError_against_null_or_empty_string_value_for_the_error_field_it_should_throw_with_descriptive_message(string expectedField)
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
                .And.OnlyHaveError(expectedField, "*required*");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*<null> or empty field name*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void When_asserting_bad_request_response_to_be_BadRequest_And_OnlyHaveError_against_null_or_empty_string_value_for_the_error_message_it_should_throw_with_descriptive_message(string expectedWildcardErrorMessage)
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
                .And.OnlyHaveError("Author", expectedWildcardErrorMessage);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*<null> or empty wildcard error message*");
        }
        #endregion
    }
}

