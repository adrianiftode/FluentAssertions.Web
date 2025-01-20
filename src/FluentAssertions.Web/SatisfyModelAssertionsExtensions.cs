﻿#if !FAV8
// ReSharper disable once CheckNamespace
namespace FluentAssertions;

/// <summary>
/// Contains extension methods for custom assertions in unit tests related to Http Response Message.
/// </summary>
[DebuggerNonUserCode]
public static class SatisfyModelAssertionsExtensions
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
    public static AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
#pragma warning disable 1573
            this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        Action<TModel> assertion,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Satisfy(assertion, because, becauseArgs);

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
    public static AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        TModel givenModelStructure,
        Action<TModel> assertion,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Satisfy(givenModelStructure, assertion, because, becauseArgs);

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
    public static AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        Func<TModel, Task> assertion,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Satisfy(assertion, because, becauseArgs);

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
    public static AndConstraint<HttpResponseMessageAssertions> Satisfy<TModel>(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        TModel givenModelStructure,
        Func<TModel, Task> assertion,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Satisfy(givenModelStructure, assertion, because, becauseArgs);
}
#endif