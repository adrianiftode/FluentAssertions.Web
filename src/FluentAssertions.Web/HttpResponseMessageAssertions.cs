using FluentAssertions.Formatting;
using FluentAssertions.Primitives;

namespace FluentAssertions.Web;

/// <summary>
/// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state.
/// </summary>
public partial class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
{
    static HttpResponseMessageAssertions()
    {
        Formatter.AddFormatter(new HttpResponseMessageFormatter());
        Formatter.AddFormatter(new AssertionsFailuresFormatter());
    }

    /// <summary>
    /// Initialized a new instance of the <see cref="HttpResponseMessageAssertions"/>
    /// class.
    /// </summary>
    /// <param name="value">The subject value to be asserted.</param>
    public HttpResponseMessageAssertions(HttpResponseMessage value) : base(value) { }

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => $"{nameof(HttpResponseMessage)}";

    private protected string? GetContent()
    {
        Func<Task<string?>> content = () => Subject.GetStringContent();
        return content.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
    }

    private protected (bool success, string? errorMessage) TryGetSubjectModel<TModel>(out TModel? model)
    {
        var (success, errorMessage) = TryGetSubjectModel(out var subjectModel, typeof(TModel));
        model = (TModel?)subjectModel;
        return (success, errorMessage);
    }

    private protected (bool success, string? errorMessage) TryGetSubjectModel(out object? model, Type modelType)
    {
        Func<Task<object?>> readModel = () => Subject.Content.ReadAsAsync(modelType, FluentAssertionsWebConfig.Serializer);
        try
        {
            model = readModel.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
            return (true, null);
        }
        catch (Exception ex) when (ex is DeserializationException or NotSupportedException)
        {
            model = default;
            var message = ex.Message;
            if (ex.InnerException != null)
            {
                message += $": {ex.InnerException.Message}";
            }

            return (false, message);
        }
    }

    private string[] CollectFailuresFromAssertion<TAsserted>(Action<TAsserted> assertion, TAsserted subject)
    {
        using var collectionScope = new AssertionScope();
        string[] assertionFailures;
        using (var itemScope = new AssertionScope())
        {
            try
            {
                assertion(subject);
                assertionFailures = itemScope.Discard();
            }
            catch (Exception ex)
            {
                assertionFailures = new[] { $"Expected to successfully verify an assertion, but the following exception occurred: { ex }" };
            }

        }

        foreach (var assertionFailure in assertionFailures)
        {
            collectionScope.AddPreFormattedFailure($"{assertionFailure}");
        }

        return collectionScope.Discard();
    }
}