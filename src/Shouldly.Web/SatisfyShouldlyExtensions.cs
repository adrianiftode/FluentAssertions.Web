using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Shouldly.Web.Internal;

namespace Shouldly;

/// <summary>
/// Shouldly assertions asserting that an HTTP response, or the model its content deserializes into,
/// satisfies one or more inner assertions.
/// </summary>
[ShouldlyMethods]
[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class SatisfyShouldlyExtensions
{
    /// <summary>
    /// Asserts that an HTTP response satisfies an assertion.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="assertion">An assertion about the HTTP response.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldSatisfy(this HttpResponseMessage? actual, Action<HttpResponseMessage> assertion,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        ShouldlyWebFailure.Rethrow(response,
            () => response.ShouldSatisfy(new[] { assertion }, customMessage, actualExpression));
    }

    /// <summary>
    /// Asserts that an HTTP response satisfies an asynchronous assertion.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="assertion">An assertion about the HTTP response.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldSatisfy(this HttpResponseMessage? actual, Func<HttpResponseMessage, Task> assertion,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        ShouldlyWebFailure.Rethrow(response,
            () => response.ShouldSatisfy(new[]
            {
                (Action<HttpResponseMessage>)(r => new Func<Task>(() => assertion(r))
                    .ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult())
            }, customMessage, actualExpression));
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an assertion.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="assertion">An assertion regarding the given model.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldSatisfy<TModel>(this HttpResponseMessage? actual, Action<TModel> assertion,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        SatisfyModel(response, new[] { assertion }, customMessage, $"{actualExpression}.Content");
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an assertion starting from an
    /// inferred model structure.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="givenModelStructure">
    /// A proposed model structure that will help to compose the assertions. This is used to define the type
    /// of the asserted model and it doesn't have to contain other values than the default one.
    /// </param>
    /// <param name="assertion">An assertion regarding the given model.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldSatisfy<TModel>(this HttpResponseMessage? actual, TModel givenModelStructure,
        Action<TModel> assertion, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        _ = givenModelStructure; // Only used to infer TModel, as in the master library.
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        SatisfyModel(response, new[] { assertion }, customMessage, $"{actualExpression}.Content");
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an asynchronous assertion.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="assertion">An assertion regarding the given model.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldSatisfy<TModel>(this HttpResponseMessage? actual, Func<TModel, Task> assertion,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        SatisfyModel(response, new[]
        {
            (Action<TModel>)(model => new Func<Task>(() => assertion(model))
                .ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult())
        }, customMessage, $"{actualExpression}.Content");
    }

    /// <summary>
    /// Asserts that an HTTP response content can be a model that satisfies an asynchronous assertion starting
    /// from an inferred model structure.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="givenModelStructure">
    /// A proposed model structure that will help to compose the assertions. This is used to define the type
    /// of the asserted model and it doesn't have to contain other values than the default one.
    /// </param>
    /// <param name="assertion">An assertion regarding the given model.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldSatisfy<TModel>(this HttpResponseMessage? actual, TModel givenModelStructure,
        Func<TModel, Task> assertion, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        _ = givenModelStructure; // Only used to infer TModel, as in the master library.
        Guard.ThrowIfArgumentIsNull(assertion, nameof(assertion),
            "Cannot verify the subject satisfies a `null` assertion.");
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        SatisfyModel(response, new[]
        {
            (Action<TModel>)(model => new Func<Task>(() => assertion(model))
                .ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult())
        }, customMessage, $"{actualExpression}.Content");
    }

    /// <summary>
    /// Deserializes the response into a model and runs the inner assertions against it, appending the
    /// HTTP response dump to any failure.
    /// </summary>
    private static void SatisfyModel<TModel>(HttpResponseMessage response, Action<TModel>[] assertions,
        string? customMessage, string actualExpression)
    {
        var (success, errorMessage, model) = response.TryReadModel(typeof(TModel));
        if (!success)
        {
            // Shouldly's ExpectedActualShouldlyMessage drops the actual value ("but was" section) when the
            // method name is ShouldSatisfy, which would hide the deserialization error. Report the failure
            // with the ShouldBeAs layout, shared with Task 07.
            throw ShouldlyWebFailure.Deserialization(response, typeof(TModel), errorMessage, customMessage,
                "ShouldBeAs", actualExpression);
        }

        var subjectModel = model is null ? default : (TModel)model;
        // The null-forgiving operator makes the native generic infer TModel (not TModel?), which matches
        // Action<TModel>[] exactly; a null subject model is still passed through, as in the master library.
        ShouldlyWebFailure.Rethrow(response,
            () => subjectModel!.ShouldSatisfy(assertions, customMessage, actualExpression));
    }
}