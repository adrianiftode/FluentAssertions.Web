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

        /// <summary>
        /// Asserts that a Bad Request HTTP response content contains an error message identifiable by an expected field name and a wildcard error text.
        /// </summary>
        /// <param name="expectedErrorField">
        /// The expected field name.
        /// </param>
        /// <param name="expectedWildcardErrorMessage">
        /// The wildcard pattern with which the error field associated error message is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<BadRequestAssertions> HaveError(string expectedErrorField, string expectedWildcardErrorMessage,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNullOrEmpty(expectedErrorField, nameof(expectedErrorField), "Cannot verify having an error against a <null> or empty field name.");
            Guard.ThrowIfArgumentIsNullOrEmpty(expectedWildcardErrorMessage, nameof(expectedWildcardErrorMessage), "Cannot verify having an error against a <null> or empty wildcard error message.");

            Func<Task<JObject>> jsonFunc = () => Subject.GetJsonObject();
            var json = jsonFunc.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(json.HasKey(expectedErrorField))
                .FailWith("Expected {context:response} " +
                          "to contain an error message related to the {0} field, but was not found." +
                          "{1}",
                    expectedErrorField, Subject);
            
            var values = json.GetStringValuesByKey(expectedErrorField);
            var matchFound = values.Any(headerValue => {
                using (var scope = new AssertionScope())
                {
                    headerValue.Should().Match(expectedWildcardErrorMessage);
                    return !scope.Discard().Any();
                }
            });

            Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .ForCondition(matchFound)
                        .FailWith("Expected {context:response} to contain " +
                                  "the error message {0} related to the {1} field, " +
                                  "but no such message was found in the actual error messages list: " +
                                  "{2}",
                            expectedWildcardErrorMessage,
                            expectedErrorField,
                            Subject);

            return new AndConstraint<BadRequestAssertions>(this);
        }

        /// <summary>
        /// Asserts that a Bad Request HTTP response content contains an error message identifiable by an wildcard error text.
        /// </summary>
        /// <param name="expectedWildcardErrorMessage">
        /// The wildcard pattern with which the error field associated error message is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<BadRequestAssertions> HaveErrorMessage(string expectedWildcardErrorMessage,
            string because = "", params object[] becauseArgs)
        {            
            Guard.ThrowIfArgumentIsNullOrEmpty(expectedWildcardErrorMessage, nameof(expectedWildcardErrorMessage), "Cannot verify having an error against a <null> or empty wildcard error message.");

            Func<Task<JObject>> jsonFunc = () => Subject.GetJsonObject();
            var json = jsonFunc.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();

            var allErrorsFields = json.GetChildrenKeys("errors");
            var allErrors = allErrorsFields.SelectMany(c => json.GetStringValuesByKey(c));

            var matchFound = allErrors.Any(headerValue => {
                using (var scope = new AssertionScope())
                {
                    headerValue.Should().Match(expectedWildcardErrorMessage);
                    return !scope.Discard().Any();
                }
            });

            Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .ForCondition(matchFound)
                        .FailWith("Expected {context:response} to contain " +
                                  "the error message {0}, " +
                                  "but no such message was found in the actual error messages list: " +
                                  "{1}",
                            expectedWildcardErrorMessage,
                            Subject);

            return new AndConstraint<BadRequestAssertions>(this);
        }
    }
}