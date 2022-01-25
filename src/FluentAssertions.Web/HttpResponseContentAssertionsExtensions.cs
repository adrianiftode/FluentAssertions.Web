using FluentAssertions.Web;
using System.Diagnostics;

namespace FluentAssertions
{
    /// <summary>
    /// Contains extension methods for custom assertions in unit tests related to Http Response Content Assertions
    /// </summary>
    [DebuggerNonUserCode]
    public static class HttpResponseContentAssertionsExtensions
    {
        /// <summary>
        /// Asserts that HTTP response content can be an equivalent representation of the expected model.
        /// </summary>
        /// <param name="expectedModel">
        /// The expected model.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public static AndConstraint<HttpResponseMessageAssertions> BeAs<TModel>(
#pragma warning disable 1573
            this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
            TModel expectedModel, string because = "", params object[] becauseArgs)
            => new HttpResponseMessageAssertions(parent.Subject).BeAs(expectedModel, because, becauseArgs);

        /// <summary>
        /// Asserts that HTTP response has content that matches a wildcard pattern.
        /// </summary>
        /// <param name="expectedWildcardText">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public static AndConstraint<HttpResponseMessageAssertions> MatchInContent(
#pragma warning disable 1573
            this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
            string expectedWildcardText, string because = "", params object[] becauseArgs)
            => new HttpResponseMessageAssertions(parent.Subject).MatchInContent(expectedWildcardText, because, becauseArgs);
    }
}
