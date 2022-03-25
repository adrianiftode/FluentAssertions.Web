using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Web;

// ReSharper disable once CheckNamespace
namespace FluentAssertions
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests related to Http Response Message.
    /// </summary>
    [DebuggerNonUserCode]
    public static class SatisfyHttpResponseMessageAssertionsExtensions
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
        [CustomAssertion]
        public static AndConstraint<HttpResponseMessageAssertions> Satisfy(
#pragma warning disable 1573
            this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
            Action<HttpResponseMessage> assertion,
            string because = "", params object[] becauseArgs)
            => new HttpResponseMessageAssertions(parent.Subject).Satisfy(assertion, because, becauseArgs);

        /// <summary>
        /// Asserts that an HTTP response satisfies an asynchronous assertion.
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
        [CustomAssertion]
        public static AndConstraint<HttpResponseMessageAssertions> Satisfy(
#pragma warning disable 1573
                this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
            Func<HttpResponseMessage, Task> assertion,
            string because = "", params object[] becauseArgs)
            => new HttpResponseMessageAssertions(parent.Subject).Satisfy(assertion, because, becauseArgs);
    }
}
