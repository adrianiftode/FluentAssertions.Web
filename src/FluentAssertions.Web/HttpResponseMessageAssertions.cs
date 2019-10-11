using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Primitives;
using FluentAssertions.Web.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state.
    /// </summary>
    public partial class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
    {
        static HttpResponseMessageAssertions()
        {
            Formatter.AddFormatter(new HttpResponseMessageFormatter());
            Formatter.AddFormatter(new AssertionsFailuresFormatter());
        }

        /// <summary>
        /// Initialized a new instance of the <see cref="HttpResponseMessageAssertions"/>
        /// class.
        /// </summary>
        /// <param name="value">The subject value to be asserted.</param>
        public HttpResponseMessageAssertions(HttpResponseMessage value) => Subject = value;

        protected override string Identifier => $"{nameof(HttpResponseMessage)}";

        /// <summary>
        /// Asserts that a HTTP response has the HTTP status 200 Ok
        /// </summary>        
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be200Ok(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.OK);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be401Unauthorized(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Unauthorized);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Be405MethodNotAllowed(string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.MethodNotAllowed);
            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        protected void ExecuteSubjectNotNull(string because, object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected an HTTP {context:response} to assert{reason}, but found <null>.");
        }

        private void ExecuteStatusAssertion(string because, object[] becauseArgs, HttpStatusCode expected)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expected == Subject.StatusCode)
                .FailWith("Expected HTTP {context:response} to be {0}{reason}, but found {1}.{2}"
                    , expected, Subject.StatusCode, Subject);
        }

        protected string GetContent()
        {
            Func<Task<string>> content = () => Subject.GetStringContent();
            return content.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }

        protected TModel GetSubjectModel<TModel>()
        {
            Func<Task<TModel>> model = () => Subject.Content.ReadAsAsync<TModel>();
            return model.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }

        protected ExpandoObject GetExpandoContent()
        {
            Func<Task<ExpandoObject>> expando = Subject.GetExpandoContent;
            return expando.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }

        protected JObject GetJsonObject()
        {
            Func<Task<JObject>> jsonObject = () => Subject.GetJsonObject();
            return jsonObject.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }
    }
}