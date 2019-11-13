using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using System;
using System.Linq;
using System.Net.Http;

namespace FluentAssertions.Web
{
    public partial class HttpResponseMessageAssertions
    {
        /// <summary>
        /// Asserts that an HTTP response satisfies an assertion.
        /// </summary>
        /// <param name="assertion">
        /// An assertion about the HTTP response.
        /// </param>
        /// <remarks>
        /// The assertion can be a single assertion or a collection of assertions if the assertion action is expressed as a statement lambda.
        /// </remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Satisfy(Action<HttpResponseMessage> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            ExecuteSatisfyAssertions(assertion, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        private void ExecuteSatisfyAssertions(Action<HttpResponseMessage> assertion, string because, object[] becauseArgs)
        {
            var failuresFromAssertions = CollectFailuresFromAssertion(assertion, Subject);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:value} to satisfy one or more assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }
        }
    }
}
