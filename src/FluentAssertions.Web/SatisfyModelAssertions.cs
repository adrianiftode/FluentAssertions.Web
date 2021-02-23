using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    public partial class HttpResponseMessageAssertions
    {
        /// <summary>
        /// Asserts that an HTTP response content can be a model that satisfies an assertion.
        /// </summary>
        /// <param name="assertion">
        /// An assertion regarding the given model.
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
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
            Action<TModel> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
                "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            ExecuteSatisfyModelAssertions(assertion, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response content can be a model that satisfies an assertion starting from an inferred model structure.
        /// </summary>
        /// <param name="givenModelStructure">
        /// A proposed model structure that will help to compose the assertions. This is used to defined the type of the asserted model and it doesn't have to contain other values than the default one.
        /// </param>
        /// <remarks>
        /// The assertion can be a single assertion or a collection of assertions if the assertion action is expressed as a statement lambda.
        /// </remarks>
        /// <param name="assertion">
        /// An assertion or a collection of assertions regarding the given model.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(TModel givenModelStructure,
            Action<TModel> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            ExecuteSatisfyModelAssertions(assertion, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response content can be a model that satisfies an asynchronous assertion.
        /// </summary>
        /// <param name="assertion">
        /// An assertion regarding the given model.
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
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
            Func<TModel, Task> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
                "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            ExecuteSatisfyModelAssertions<TModel>(asserted =>
            {
                Func<Task> assertionExecutor = () => assertion(asserted);
                assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
            }, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        /// <summary>
        /// Asserts that an HTTP response content can be a model that satisfies an asynchronous assertion starting from an inferred model structure.
        /// </summary>
        /// <param name="givenModelStructure">
        /// A proposed model structure that will help to compose the assertions. This is used to defined the type of the asserted model and it doesn't have to contain other values than the default one.
        /// </param>
        /// <remarks>
        /// The assertion can be a single assertion or a collection of assertions if the assertion action is expressed as a statement lambda.
        /// </remarks>
        /// <param name="assertion">
        /// An assertion or a collection of assertions regarding the given model.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(TModel givenModelStructure,
            Func<TModel, Task> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");
            ExecuteSubjectNotNull(because, becauseArgs);

            ExecuteSatisfyModelAssertions<TModel>(model =>
            {
                Func<Task> assertionExecutor = () => assertion(model);
                assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
            }, because, becauseArgs);

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        private protected void ExecuteSatisfyModelAssertions<TModel>(Action<TModel> assertion, string because, object[] becauseArgs)
        {
            var (success, errorMessage) = TryGetSubjectModel<TModel>(out var model);

            ExecuteModelExtractedAssertion<TModel>(success, errorMessage, because, becauseArgs);

            var failuresFromAssertions = CollectFailuresFromAssertion(assertion, model);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:value} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }
        }
    }
}
