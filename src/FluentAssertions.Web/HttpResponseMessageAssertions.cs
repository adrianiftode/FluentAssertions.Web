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
    public class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
    {
        static HttpResponseMessageAssertions()
        {
            Formatter.AddFormatter(new HttpResponseMessageFormatter());
        }

        public HttpResponseMessageAssertions(HttpResponseMessage value) => Subject = value;

        protected override string Identifier => $"{nameof(HttpResponseMessage)}";

        public AndConstraint<OkAssertions> Be200Ok(string because = "", params object[] becauseArgs)
        {
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.OK);

            return new AndConstraint<OkAssertions>(new OkAssertions(Subject));
        }

        public AndWhichConstraint<OkAssertions, TModel> Be200Ok<TModel>(TModel expected, string because = "",
            params object[] becauseArgs)
        {
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.OK);

            var subjectModel = GetSubjectModel<TModel>();

            return new AndWhichConstraint<OkAssertions, TModel>(new OkAssertions(Subject), subjectModel);
        }

        public AndConstraint<HttpResponseMessageAssertions> Be405MethodNotAllowed(string because = "", params object[] becauseArgs)
        {
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.MethodNotAllowed);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        public AndConstraint<BadRequestAssertions> Be400BadRequest(string because = "", params object[] becauseArgs)
        {
            ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.BadRequest);
            return new AndConstraint<BadRequestAssertions>(new BadRequestAssertions(Subject));
        }


        public AndConstraint<HttpResponseMessageAssertions> WithHttpHeader(string expectedHeader, string expectedHeaderValue,
            string because = "", params object[] becauseArgs)
        {
            bool IsHeaderPresent() => Subject.Headers.Contains(expectedHeader);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)

                .ForCondition(IsHeaderPresent())
                .FailWith("Expected {context:value} to contain " +
                          "the HttpHeader {0} with content {1}, " +
                          "but no such header was found in the actual headers list: " +
                          $"{Environment.NewLine}{{2}}{Environment.NewLine}. " +
                          $"The response content was {Environment.NewLine}{{3}}",
                    expectedHeader,
                    expectedHeaderValue,
                    Subject.GetHeaders(),
                    Subject)
                ;
            return new AndConstraint<HttpResponseMessageAssertions>(this);
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