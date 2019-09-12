using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;

namespace FluentAssertions.Web
{
    public class BadRequestAssertions : ReferenceTypeAssertions<HttpResponseMessage, BadRequestAssertions>
    {
        private readonly string _responseContent;
        private readonly ExpandoObject _responseContentExpando;

        public BadRequestAssertions(HttpResponseMessage value, string responseContent,
            ExpandoObject responseContentExpando)
        {
            Subject = value;
            _responseContent = responseContent;
            _responseContentExpando = responseContentExpando;
        }

        protected override string Identifier => "BadRequest";

        public AndConstraint<BadRequestAssertions> WithError(string expectedField, string expectedErrorMessage,
            string because = "", params object[] becauseArgs)
        {
            IDictionary<string, object> properties = _responseContentExpando;

            IReadOnlyCollection<string> ErrorsLazy() => properties != null && properties.ContainsKey(expectedField)
                ? ((List<object>)properties[expectedField]).Select(c => c.ToString()).ToList()
                : null;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)

                .ForCondition(properties?.ContainsKey(expectedField) == true)
                .FailWith("Expected {context:value} " +
                          "to contain a response with a field named {0}, but found {1}. " +
                          $"The response content was {Environment.NewLine} {{2}}",
                    expectedField, properties?.Keys, _responseContent)

                .Then
                .ForCondition(ErrorsLazy().Any(c => c.Contains(expectedErrorMessage)))
                .FailWith("Expected {context:value} to contain " +
                          "the error message {0} associated with {1}, " +
                          "but no such message was found in the actual error messages list: " +
                          $"{Environment.NewLine}{{2}}{Environment.NewLine}" +
                          $"{Environment.NewLine}{Environment.NewLine}" +
                          $"The response content was {Environment.NewLine}{{3}}",
                    expectedErrorMessage,
                    expectedField,
                    ErrorsLazy(),
                    _responseContent)
                ;
            return new AndConstraint<BadRequestAssertions>(this);
        }

        public AndConstraint<BadRequestAssertions> WithHttpHeader(string expectedHeader, string expectedHeaderValue,
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
                    Subject.Headers,
                    _responseContent)
                ;
            return new AndConstraint<BadRequestAssertions>(this);
        }
    }
}