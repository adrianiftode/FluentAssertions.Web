﻿using FluentAssertions.Execution;
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
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
            Action<TModel> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
                "Cannot verify the subject satisfies a `null` assertion.");

            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");

            var (success, errorMessage) = TryGetSubjectModel<TModel>(out var model);

            Type? modelType = typeof(TModel);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(success)
                .FailWith("Expected {context:response} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                    modelType?.ToString() ?? "unknown type", Subject, errorMessage);

            var failuresFromAssertions = CollectFailuresFromAssertion(assertion, model);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:response} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }

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
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(TModel givenModelStructure,
            Action<TModel> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");

            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");

            var (success, errorMessage) = TryGetSubjectModel<TModel>(out var model);

            Type? modelType = typeof(TModel);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(success)
                .FailWith("Expected {context:response} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                    modelType?.ToString() ?? "unknown type", Subject, errorMessage);

            var failuresFromAssertions = CollectFailuresFromAssertion(assertion, model);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:response} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }

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
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
            Func<TModel, Task> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
                "Cannot verify the subject satisfies a `null` assertion.");

            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");

            Action<TModel> assertion1 = asserted =>
            {
                Func<Task> assertionExecutor = () => assertion(asserted);
                assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
            };
            var (success, errorMessage) = TryGetSubjectModel<TModel>(out var model);

            Type? modelType = typeof(TModel);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(success)
                .FailWith("Expected {context:response} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                    modelType?.ToString() ?? "unknown type", Subject, errorMessage);

            var failuresFromAssertions = CollectFailuresFromAssertion(assertion1, model);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:response} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }

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
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(TModel givenModelStructure,
            Func<TModel, Task> assertion,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");

            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected a {context:response} to assert{reason}, but found <null>.");

            Action<TModel> assertion1 = model =>
            {
                Func<Task> assertionExecutor = () => assertion(model);
                assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
            };
            var (success, errorMessage) = TryGetSubjectModel<TModel>(out var model1);

            Type? modelType = typeof(TModel);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(success)
                .FailWith("Expected {context:response} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                    modelType?.ToString() ?? "unknown type", Subject, errorMessage);

            var failuresFromAssertions = CollectFailuresFromAssertion(assertion1, model1);

            if (failuresFromAssertions.Any())
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:response} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), Subject);
            }

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
    }
}
