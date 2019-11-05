using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using System;
using System.Linq.Expressions;
using System.Net.Http;

namespace FluentAssertions.Web
{
    public partial class HttpResponseMessageAssertions
    {
        /// <summary>
        /// Asserts that an HTTP response satisfies a condition about it.
        /// </summary>
        /// <remarks>
        /// This assertions should be used only when there is not other available assertion, but it still needed to benefit from logging the HTTP response and the originated HTTP request when it fails.
        /// </remarks>
        /// <param name="predicate">
        /// A predicate to match the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Satisfy(Expression<Func<HttpResponseMessage, bool>> predicate,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate), "Cannot verify the subject satisfies a predicate that is `null`.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var compiledPredicate = predicate.Compile();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(compiledPredicate(Subject))
                .FailWith("Expected {context:value} to satisfy condition {0}, but it was not satisfied{reason}.{1}",
                    predicate.Body,
                    Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
    }
}
