using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state related to HTTP Bad Request response
    /// </summary>
    public class BadRequestAssertions : HttpResponseMessageAssertions
    {
        /// <summary>
        /// Initialized a new instance of the <see cref="BadRequestAssertions"/>
        /// class.
        /// </summary>
        /// <param name="value">The subject value to be asserted.</param>
        public BadRequestAssertions(HttpResponseMessage value) : base(value)
        {
        }

        protected override string Identifier => "BadRequest";

        public AndConstraint<BadRequestAssertions> HaveError(string expectedField, string expectedErrorMessage,
            string because = "", params object[] becauseArgs)
        {
            Func<Task<JObject>> jsonFunc = () => Subject.GetJsonObject();
            var json = jsonFunc.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(json.HasKey(expectedField))
                .FailWith("Expected {context:response} " +
                          "to contain an error message related to the {0} field, but was not found." +
                          "{1}",
                    expectedField, Subject)

                .Then
                .ForCondition(json.GetStringValuesByKey(expectedField).Contains(expectedErrorMessage))
                .FailWith("Expected {context:response} to contain " +
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