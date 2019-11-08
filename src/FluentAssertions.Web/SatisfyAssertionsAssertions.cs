using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FluentAssertions.Web
{
    public partial class HttpResponseMessageAssertions
    {
        /// <summary>
        /// Asserts that an HTTP response satisfies an assertion.
        /// </summary>
        /// <param name="responseAssertion">
        /// An assertion about the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> SatisfyAssertions(Action<HttpResponseMessage> responseAssertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertion, nameof(responseAssertion), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var responseAssertions = new[] { responseAssertion };

            ExecuteSatisfyAssertions(responseAssertions, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response satisfies a collection of assertions.
        /// </summary>
        /// <param name="responseAssertion1">
        /// Assertion 1 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion2">
        /// Assertion 2 about the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> SatisfyAssertions(
            Action<HttpResponseMessage> responseAssertion1,
            Action<HttpResponseMessage> responseAssertion2,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertion1, nameof(responseAssertion1), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion2, nameof(responseAssertion2), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var responseAssertions = new[] { responseAssertion1, responseAssertion2 };

            ExecuteSatisfyAssertions(responseAssertions, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response satisfies a collection of assertions.
        /// </summary>
        /// <param name="responseAssertion1">
        /// Assertion 1 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion2">
        /// Assertion 2 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion3">
        /// Assertion 3 about the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> SatisfyAssertions(
            Action<HttpResponseMessage> responseAssertion1,
            Action<HttpResponseMessage> responseAssertion2,
            Action<HttpResponseMessage> responseAssertion3,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertion1, nameof(responseAssertion1), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion2, nameof(responseAssertion2), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion3, nameof(responseAssertion3), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var responseAssertions = new[] { responseAssertion1, responseAssertion2, responseAssertion3 };

            ExecuteSatisfyAssertions(responseAssertions, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response satisfies a collection of assertions.
        /// </summary>
        /// <param name="responseAssertion1">
        /// Assertion 1 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion2">
        /// Assertion 2 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion3">
        /// Assertion 3 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion4">
        /// Assertion 4 about the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> SatisfyAssertions(
            Action<HttpResponseMessage> responseAssertion1,
            Action<HttpResponseMessage> responseAssertion2,
            Action<HttpResponseMessage> responseAssertion3,
            Action<HttpResponseMessage> responseAssertion4,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertion1, nameof(responseAssertion1), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion2, nameof(responseAssertion2), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion3, nameof(responseAssertion3), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion4, nameof(responseAssertion4), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var responseAssertions = new[] { responseAssertion1, responseAssertion2, responseAssertion3, responseAssertion4 };

            ExecuteSatisfyAssertions(responseAssertions, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response satisfies a collection of assertions.
        /// </summary>
        /// <param name="responseAssertion1">
        /// Assertion 1 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion2">
        /// Assertion 2 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion3">
        /// Assertion 3 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion4">
        /// Assertion 4 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion5">
        /// Assertion 5 about the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> SatisfyAssertions(
            Action<HttpResponseMessage> responseAssertion1,
            Action<HttpResponseMessage> responseAssertion2,
            Action<HttpResponseMessage> responseAssertion3,
            Action<HttpResponseMessage> responseAssertion4,
            Action<HttpResponseMessage> responseAssertion5,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertion1, nameof(responseAssertion1), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion2, nameof(responseAssertion2), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion3, nameof(responseAssertion3), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion4, nameof(responseAssertion4), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion5, nameof(responseAssertion5), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var responseAssertions = new[] { responseAssertion1, responseAssertion2, responseAssertion3, responseAssertion4, responseAssertion5 };

            ExecuteSatisfyAssertions(responseAssertions, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response satisfies a collection of assertions.
        /// </summary>
        /// <param name="responseAssertion1">
        /// Assertion 1 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion2">
        /// Assertion 2 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion3">
        /// Assertion 3 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion4">
        /// Assertion 4 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion5">
        /// Assertion 5 about the HTTP response.
        /// </param>
        /// <param name="responseAssertion6">
        /// Assertion 6 about the HTTP response.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> SatisfyAssertions(
            Action<HttpResponseMessage> responseAssertion1,
            Action<HttpResponseMessage> responseAssertion2,
            Action<HttpResponseMessage> responseAssertion3,
            Action<HttpResponseMessage> responseAssertion4,
            Action<HttpResponseMessage> responseAssertion5,
            Action<HttpResponseMessage> responseAssertion6,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertion1, nameof(responseAssertion1), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion2, nameof(responseAssertion2), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion3, nameof(responseAssertion3), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion4, nameof(responseAssertion4), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion5, nameof(responseAssertion5), "Cannot verify the subject satisfies a `null` assertion.");
            Guard.ThrowIfArgumentIsNull(responseAssertion6, nameof(responseAssertion6), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            var responseAssertions = new[] { responseAssertion1, responseAssertion2, responseAssertion3, responseAssertion4, responseAssertion5 };

            ExecuteSatisfyAssertions(responseAssertions, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        private void ExecuteSatisfyAssertions(Action<HttpResponseMessage>[] responseAssertions, string because, object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(responseAssertions, nameof(responseAssertions), "Cannot verify the subject satisfies against a collection of `null` assertions.");

            var elementAssertions = responseAssertions.ToList();
            if (!elementAssertions.Any())
            {
                throw new ArgumentException("Cannot verify against an empty collection of assertions",
                    nameof(responseAssertions));
            }

            var failuresFromAssertions = CollectFailuresFromAssertions(elementAssertions);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:value} to satisfy "
                                    + (responseAssertions.Length == 1 ? "assertion" : "all assertions")
                                    + "{reason}, but "
                                    + (responseAssertions.Length == 1 ? "is not satisfied" : "some assertions are not satisfied")
                                    + ": {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }
        }

        private string[] CollectFailuresFromAssertions(IEnumerable<Action<HttpResponseMessage>> elementAssertions)
        {
            string[] collectionFailures;
            using (var collectionScope = new AssertionScope())
            {
                foreach (var assertion in elementAssertions)
                {
                    string[] assertionFailures;
                    using (var itemScope = new AssertionScope())
                    {
                        assertion(Subject);
                        assertionFailures = itemScope.Discard();
                    }

                    if (assertionFailures.Length > 0)
                    {
                        var failures = string.Join(Environment.NewLine, assertionFailures.Select(x => x.TrimEnd('.')));
                        collectionScope.AddPreFormattedFailure($"{failures}");
                    }
                }

                collectionFailures = collectionScope.Discard();
            }

            return collectionFailures;
        }
    }
}
