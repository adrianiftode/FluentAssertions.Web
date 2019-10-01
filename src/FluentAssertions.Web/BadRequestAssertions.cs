using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    public class BadRequestAssertions : HttpResponseMessageAssertions
    {
        public BadRequestAssertions(HttpResponseMessage value) : base(value)
        {

        }

        protected override string Identifier => "BadRequest";

        public AndConstraint<BadRequestAssertions> WithError(string expectedField, string expectedErrorMessage,
            string because = "", params object[] becauseArgs)
        {
            Func<Task<JObject>> jsonFunc = () => Subject.GetJsonObject();
            var json = jsonFunc.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(json.HasKey(expectedField))
                .FailWith("Expected {context:value} " +
                          "to an error message related to the `{0}` field, but not was found." +
                          "{1}",
                    expectedField, Subject)

                .Then
                .ForCondition(json.GetStringValuesByKey(expectedField).Contains(expectedErrorMessage))
                .FailWith("Expected {context:value} to contain " +
                          "the error message {0} related to the {1} field, " +
                          "but no such message was found in the actual error messages list: " +
                          "{2}",
                    expectedErrorMessage,
                    expectedField,
                    Subject)
                ;
            return new AndConstraint<BadRequestAssertions>(this);
        }
    }
}