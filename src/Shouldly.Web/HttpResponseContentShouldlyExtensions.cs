using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Shouldly.Web.Internal;

namespace Shouldly;

/// <summary>
/// Shouldly assertions on the body content of an <see cref="HttpResponseMessage"/>.
/// </summary>
[ShouldlyMethods]
[DebuggerStepThrough]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class HttpResponseContentShouldlyExtensions
{
    /// <summary>
    /// Asserts that an HTTP response body is null or an empty string.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldBeEmpty(this HttpResponseMessage? actual, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);
        var content = response.ReadContentAsString();

        ShouldlyWebFailure.Rethrow(response,
            () => content.ShouldBeNullOrEmpty(customMessage, $"{actualExpression}.Content"));
    }

    /// <summary>
    /// Asserts that an HTTP response body is an equivalent representation of the expected model.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="expectedModel">The expected model.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldBeAs<TModel>(this HttpResponseMessage? actual, TModel expectedModel, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
        => ShouldBeAsCore(actual, expectedModel, (EquivalencyOptions?)null, customMessage, actualExpression);

    /// <summary>
    /// Asserts that an HTTP response body is an equivalent representation of the expected model,
    /// comparing the object graphs with the specified <see cref="EquivalencyOptions"/>.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="expectedModel">The expected model.</param>
    /// <param name="options">Rules that influence how the object graphs are compared.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldBeAs<TModel>(this HttpResponseMessage? actual, TModel expectedModel, EquivalencyOptions options,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        Guard.ThrowIfArgumentIsNull(options, nameof(options));
        ShouldBeAsCore(actual, expectedModel, options, customMessage, actualExpression);
    }

    /// <summary>
    /// Asserts that an HTTP response body matches a wildcard pattern, where <c>*</c> matches any
    /// run of characters and <c>?</c> matches exactly one.
    /// </summary>
    /// <param name="actual">The HTTP response under test.</param>
    /// <param name="expectedWildcardText">The wildcard pattern with which the body is matched.</param>
    /// <param name="customMessage">Extra text shown under <c>Additional Info</c> when the assertion fails.</param>
    /// <param name="actualExpression">Captured by the compiler; do not pass it.</param>
    public static void ShouldMatchInContent(this HttpResponseMessage? actual, string expectedWildcardText,
        string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        Guard.ThrowIfArgumentIsNull(expectedWildcardText, nameof(expectedWildcardText),
            "Cannot verify an HTTP response content match a <null> wildcard pattern.");

        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);
        var content = response.ReadContentAsString();

        if (string.IsNullOrEmpty(content) || !content.WildcardMatch(expectedWildcardText))
        {
            throw ShouldlyWebFailure.Create(response, new ExpectedActualShouldlyMessage(
                expectedWildcardText,
                content,
                customMessage,
                shouldlyMethod: nameof(ShouldMatchInContent),
                actualExpression: $"{actualExpression}.Content"));
        }
    }

    private static void ShouldBeAsCore<TModel>(this HttpResponseMessage? actual, TModel expectedModel,
        EquivalencyOptions? options, string? customMessage, string? actualExpression)
    {
        var response = ResponseGuard.EnsureNotNull(actual, customMessage, actualExpression);

        if (expectedModel is null)
        {
            throw new ArgumentNullException(nameof(expectedModel),
                "Cannot verify having a content equivalent to a model against a <null> model.");
        }

        var expectedModelType = expectedModel.GetType();
        var (success, errorMessage, model) = response.TryReadModel(expectedModelType);

        if (!success)
        {
            throw ShouldlyWebFailure.Create(response, new ExpectedActualShouldlyMessage(
                expectedModelType,
                errorMessage,
                customMessage,
                shouldlyMethod: nameof(ShouldBeAs),
                actualExpression: $"{actualExpression}.Content"));
        }

        ShouldlyWebFailure.Rethrow(response,
            () => model.ShouldBeEquivalentTo(expectedModel, options ?? new EquivalencyOptions(), customMessage,
                $"{actualExpression}.Content"));
    }
}