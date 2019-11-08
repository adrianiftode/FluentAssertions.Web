﻿using FluentAssertions.Execution;
using System.Net;

namespace FluentAssertions.Web
{
    public partial class HttpResponseMessageAssertions
    {
        #region Be1XXInformational
        /// <summary>
        /// Asserts that a HTTP response has a HTTP status code representing an informational response.
        /// </summary>
        /// <remarks>The HTTP response was an informational one if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 100-199.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be1XXInformational(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.StatusCode < HttpStatusCode.OK)
                .FailWith("Expected {context:response} to have a HTTP status code representing an informational error, but it was {0}{reason}.{1}",
                    Subject.StatusCode, Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region Be2XXSuccessful
        /// <summary>
        /// Asserts that a HTTP response has a successful HTTP status code.
        /// </summary>
        /// <remarks>The HTTP response was successful if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 200-299.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be2XXSuccessful(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.IsSuccessStatusCode)
                .FailWith("Expected {context:response} to have a successful HTTP status code, but it was {0}{reason}.{1}",
                    Subject.StatusCode, Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region Be3XXRedirection
        /// <summary>
        /// Asserts that a HTTP response has a HTTP status code representing a redirection response.
        /// </summary>
        /// <remarks>The HTTP response was an informational one if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 300-399.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be3XXRedirection(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.StatusCode >= HttpStatusCode.Moved && Subject.StatusCode < HttpStatusCode.BadRequest)
                .FailWith("Expected {context:response} to have a HTTP status code representing a redirection, but it was {0}{reason}.{1}",
                    Subject.StatusCode, Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region Be4XXClientError
        /// <summary>
        /// Asserts that a HTTP response has a HTTP status code representing a client error.
        /// </summary>
        /// <remarks>The HTTP response was a client error if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 400-499.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be4XXClientError(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.StatusCode >= HttpStatusCode.BadRequest && Subject.StatusCode < HttpStatusCode.InternalServerError)
                .FailWith("Expected {context:response} to have a HTTP status code representing a client error, but it was {0}{reason}.{1}",
                    Subject.StatusCode, Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region Be5XXServerError
        /// <summary>
        /// Asserts that a HTTP response has a HTTP status code representing a server error.
        /// </summary>
        /// <remarks>The HTTP response was a server error if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was above 500.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be5XXServerError(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.StatusCode >= HttpStatusCode.InternalServerError)
                .FailWith("Expected {context:response} to have a HTTP status code representing a server error, but it was {0}{reason}.{1}",
                    Subject.StatusCode, Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region HaveHtppStatus
        /// <summary>
        /// Asserts that a HTTP response has a HTTP status with the specified code.
        /// </summary>
        /// <param name="expected">
        /// The code of the expected HTTP Status.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> HaveHttpStatusCode(HttpStatusCode expected, string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, expected);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region NotHaveHtppStatus
        /// <summary>
        /// Asserts that a HTTP response does not have a HTTP status with the specified code.
        /// </summary>
        /// <param name="unexpected">
        /// The code of the unexpected HTTP Status.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> NotHaveHttpStatusCode(HttpStatusCode unexpected, string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(unexpected != Subject.StatusCode)
                .FailWith("Did not expect {context:response} to have status {0}{reason}.{1}",
                    Subject.StatusCode, Subject);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion

        #region BeXXXHttpStatus
        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 100 Continue
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be100Continue(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Continue);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 101 Switching Protocols
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be101SwitchingProtocols(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.SwitchingProtocols);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 200 Ok
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be200Ok(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.OK);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 201 Created
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be201Created(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Created);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 202 Accepted
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be202Accepted(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Accepted);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 203 Non Authoritative Information
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be203NonAuthoritativeInformation(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NonAuthoritativeInformation);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 204 No Content
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be204NoContent(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NoContent);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 205 Reset Content
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be205ResetContent(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.ResetContent);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 206 Partial Content
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be206PartialContent(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.PartialContent);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 300 Multiple Choices
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be300MultipleChoices(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.MultipleChoices, "MultipleChoices");
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 300 Ambiguous
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be300Ambiguous(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Ambiguous);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 301 Moved Permanently
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be301MovedPermanently(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.MovedPermanently, "MovedPermanently");
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 301 Moved
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be301Moved(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Moved);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 302 Found
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be302Found(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Found, "Found");
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 302 Redirect
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be302Redirect(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Redirect, "Redirect");
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 303 See Other
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be303SeeOther(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.SeeOther, "SeeOther");
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 303 Redirect Method
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be303RedirectMethod(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.RedirectMethod);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 304 Not Modified
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be304NotModified(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NotModified);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 305 Use Proxy
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be305UseProxy(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.UseProxy);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 306 Unused
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be306Unused(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Unused);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 307 Temporary Redirect
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be307TemporaryRedirect(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.TemporaryRedirect);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 307 Redirect Keep Verb
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be307RedirectKeepVerb(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.RedirectKeepVerb, "RedirectKeepVerb");
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 400 BadRequest
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<BadRequestAssertions> Be400BadRequest(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.BadRequest);
            return new AndConstraint<BadRequestAssertions>(new BadRequestAssertions(Subject));
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 401 Unauthorized
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be401Unauthorized(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Unauthorized);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 402 Payment Required
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be402PaymentRequired(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.PaymentRequired);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 403 Forbidden
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be403Forbidden(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Forbidden);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 404 Not Found
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be404NotFound(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NotFound);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 405 Method Not Allowed
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be405MethodNotAllowed(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.MethodNotAllowed);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 406 Not Acceptable
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be406NotAcceptable(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NotAcceptable);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 407 Proxy Authentication Required
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be407ProxyAuthenticationRequired(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.ProxyAuthenticationRequired);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 408 Request Timeout
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be408RequestTimeout(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.RequestTimeout);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 409 Conflict
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be409Conflict(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Conflict);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 410 Gone
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be410Gone(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Gone);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 411 Length Required
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be411LengthRequired(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.LengthRequired);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 412 Precondition Failed
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be412PreconditionFailed(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.PreconditionFailed);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 413 Request Entity Too Large
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be413RequestEntityTooLarge(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.RequestEntityTooLarge);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 414 Request Uri Too Long
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be414RequestUriTooLong(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.RequestUriTooLong);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 415 Unsupported Media Type
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be415UnsupportedMediaType(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.UnsupportedMediaType);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 416 Requested Range Not Satisfiable
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be416RequestedRangeNotSatisfiable(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.RequestedRangeNotSatisfiable);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 417 Expectation Failed
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be417ExpectationFailed(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.ExpectationFailed);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 426 UpgradeRequired
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be426UpgradeRequired(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.UpgradeRequired);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 500 Internal Server Error
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be500InternalServerError(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.InternalServerError);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 501 Not Implemented
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be501NotImplemented(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NotImplemented);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 502 Bad Gateway
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be502BadGateway(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.BadGateway);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 503 Service Unavailable
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be503ServiceUnavailable(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.ServiceUnavailable);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 504 Gateway Timeout
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be504GatewayTimeout(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.GatewayTimeout);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 505 Http Version Not Supported
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be505HttpVersionNotSupported(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.HttpVersionNotSupported);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
        #endregion
    }
}