using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using System;

namespace FluentAssertions.Web
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state related to the HTTP content.
    /// </summary>
    public partial class HttpResponseMessageAssertions
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
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> HaveContent<TModel>(TModel expectedModel, string because = "", params object[] becauseArgs)
        {
            ExecuteSubjectNotNull(because, becauseArgs);

            if (expectedModel == null)
            {
                throw new ArgumentNullException(nameof(expectedModel), "Cannot verify having a content against a <null> content.");
            }

            var success = TryGetSubjectModel<TModel>(out var subjectModel);

            Execute.Assertion
                     .BecauseOf(because, becauseArgs)
                     .ForCondition(success)
                     .FailWith("Expected {context:response} to have a content equivalent to a model, but the JSON respresentation could not be parsed{reason}. {0}",
                         Subject);

            string[] failures;

            using (var scope = new AssertionScope())
            {
                subjectModel.Should().BeEquivalentTo(expectedModel);

                failures = scope.Discard();
            }

            Execute.Assertion
                       .BecauseOf(because, becauseArgs)
                       .ForCondition(failures.Length == 0)
                       .FailWith("Expected {context:response} to have a content equivalent to a model, but is has differences:{0}{reason}. {1}",
                           new AssertionsFailures(failures),
                           Subject);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
    }
}