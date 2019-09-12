using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    public static class HttpResponseMessageFluentAssertionsExtensions
    {
        public static HttpResponseMessageExtendedAssertions Should(this HttpResponseMessage actual)
            => new HttpResponseMessageExtendedAssertions(actual);
    }

    public class HttpResponseMessageExtendedAssertions : ReferenceTypeAssertions<HttpResponseMessage,
        HttpResponseMessageExtendedAssertions>
    {
        public HttpResponseMessageExtendedAssertions(HttpResponseMessage value)
        {
            Subject = value;
        }

        protected override string Identifier => $"{nameof(HttpResponseMessageExtendedAssertions)}";

        public async Task<AndConstraint<ObjectAssertions>> BeOk<TModel>(string because = "",
            params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.OK);

            var subjectModel = await Subject.Content.ReadAsAsync<TModel>();
            return new AndConstraint<ObjectAssertions>(new ObjectAssertions(subjectModel));
        }

        public async Task BeOk(string because = "",
            params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.OK);
        }

        public async Task<AndConstraint<ObjectAssertions>> BeCreated<TModel>(string because = "",
            params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Created);

            var subjectModel = await Subject.Content.ReadAsAsync<TModel>();
            return new AndConstraint<ObjectAssertions>(new ObjectAssertions(subjectModel));
        }

        public async Task<BadRequestAssertions> BeBadRequest(string because = "", params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.BadRequest);

            var responseContent = await Subject.GetStringContent();
            var expandoContent = await Subject.GetExpandoContent();

            return new BadRequestAssertions(Subject, responseContent, expandoContent);
        }

        public async Task BeNotFound(string because = "", params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.NotFound);
        }

        public async Task BeForbidden(string because = "", params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.Forbidden);
        }

        public async Task BeMethodNotAllowed(string because = "",
            params object[] becauseArgs)
        {
            await ExecuteStatusAssertion(because, becauseArgs, HttpStatusCode.MethodNotAllowed);
        }

        private async Task ExecuteStatusAssertion(string because, object[] becauseArgs, HttpStatusCode expected)
        {
            var content = await Subject.GetBeautifiedStringContent();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expected == Subject.StatusCode)
                .FailWith("Expected {0}{because}, but found {1}{reason}. The content was {2}."
                    , expected, Subject.StatusCode, content);
        }
    }

    public class ContentAssertions : ReferenceTypeAssertions<string, ContentAssertions>
    {
        public ContentAssertions(string content)
        {
            Subject = content;
        }

        protected override string Identifier => "content";
    }

    public class CreatedAssertions<TModel> : ReferenceTypeAssertions<HttpResponseMessage, CreatedAssertions<TModel>>
    {
        private readonly TModel _model;
        private readonly string _responseContent;
        private readonly ExpandoObject _responseContentExpando;
        public CreatedAssertions(HttpResponseMessage value, TModel model, string responseContent, ExpandoObject responseContentExpando)
        {
            Subject = value;
            _model = model;
            _responseContent = responseContent;
            _responseContentExpando = responseContentExpando;
        }

        protected override string Identifier => "Created";
    }
}