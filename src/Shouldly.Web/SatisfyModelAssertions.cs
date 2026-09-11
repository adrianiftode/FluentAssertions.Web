// ReSharper disable CheckNamespace
namespace Shouldly;

[ShouldlyMethods]
public static partial class HttpResponseMessageAssertions
{
    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an assertion.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
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
    public static void ShouldSatisfy<TModel>(
        this HttpResponseMessage? actual,
        Action<TModel> assertion,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");

        ExecuteAssertion
            .ForCondition(actual is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        var (success, errorMessage) = actual!.TryGetSubjectModel<TModel>(out var model);

        Type? modelType = typeof(TModel);

        ExecuteAssertion
            .ForCondition(success)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                modelType?.ToString() ?? "unknown type", new FormatHttpResponseMessage(actual), errorMessage);

        var failuresFromAssertions = assertion!.CollectFailuresFromAssertion(model);

        if (failuresFromAssertions.Any())
        {
            ExecuteAssertion
                .ForCondition(false)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:actual} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                    new AssertionsFailures(failuresFromAssertions), new FormatHttpResponseMessage(actual));
        }
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an assertion starting from an inferred model structure.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
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
    public static void ShouldSatisfy<TModel>(
        this HttpResponseMessage? actual,
        TModel givenModelStructure,
        Action<TModel> assertion,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");

        ExecuteAssertion
            .ForCondition(actual is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        var (success, errorMessage) = actual!.TryGetSubjectModel<TModel>(out var model);

        Type? modelType = typeof(TModel);

        ExecuteAssertion
            .ForCondition(success)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                modelType?.ToString() ?? "unknown type", new FormatHttpResponseMessage(actual), errorMessage);

        var failuresFromAssertions = assertion!.CollectFailuresFromAssertion(model);

        if (failuresFromAssertions.Any())
        {
            ExecuteAssertion
                .ForCondition(false)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:actual} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                    new AssertionsFailures(failuresFromAssertions), new FormatHttpResponseMessage(actual));
        }
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an asynchronous assertion.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
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
    public static void ShouldSatisfy<TModel>(
        this HttpResponseMessage? actual,
        Func<TModel, Task> assertion,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");

        ExecuteAssertion
            .ForCondition(actual is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        // ReSharper disable once ConvertToLocalFunction
#pragma warning disable IDE0039 // Use local function
        Action<TModel> assertionCaller = asserted =>
#pragma warning restore IDE0039 // Use local function
        {
            Func<Task> assertionExecutor = () => assertion(asserted);
            assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        };
        var (success, errorMessage) = actual!.TryGetSubjectModel<TModel>(out var model);

        Type? modelType = typeof(TModel);

        ExecuteAssertion
            .ForCondition(success)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                modelType?.ToString() ?? "unknown type", new FormatHttpResponseMessage(actual), errorMessage);

        var failuresFromAssertions = assertionCaller!.CollectFailuresFromAssertion(model);

        if (failuresFromAssertions.Any())
        {
            ExecuteAssertion
                .ForCondition(false)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:actual} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                    new AssertionsFailures(failuresFromAssertions), new FormatHttpResponseMessage(actual));
        }
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an asynchronous assertion starting from an inferred model structure.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
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
    public static void ShouldSatisfy<TModel>(
        this HttpResponseMessage? actual, 
        TModel givenModelStructure,
        Func<TModel, Task> assertion,
        string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion), "Cannot verify the subject satisfies a `null` assertion.");

        ExecuteAssertion
            .ForCondition(actual is not null)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        // ReSharper disable once ConvertToLocalFunction
#pragma warning disable IDE0039 // Use local function
        Action<TModel> assertionCaller = model =>
#pragma warning restore IDE0039 // Use local function
        {
            Func<Task> assertionExecutor = () => assertion(model);
            assertionExecutor.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        };
        var (success, errorMessage) = actual!.TryGetSubjectModel<TModel>(out var subjectModel);

        Type? modelType = typeof(TModel);

        ExecuteAssertion
            .ForCondition(success)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
                modelType?.ToString() ?? "unknown type", new FormatHttpResponseMessage(actual), errorMessage);

        var failuresFromAssertions = assertionCaller!.CollectFailuresFromAssertion(subjectModel);

        if (failuresFromAssertions.Any())
        {
            ExecuteAssertion
                .ForCondition(false)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:actual} to satisfy one or more model assertions, but it wasn't{reason}: {0}{1}",
                        new AssertionsFailures(failuresFromAssertions), new FormatHttpResponseMessage(actual));
        }
    }
}
