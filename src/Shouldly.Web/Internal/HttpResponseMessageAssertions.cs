// ReSharper disable CheckNamespace
using System.Collections.Generic;

namespace Shouldly.Web.Internal;

/// <summary>
/// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state.
/// </summary>
internal static class HttpResponseMessageExtensions
{
    public static string? GetContent(this HttpResponseMessage subject)
    {
        Func<Task<string?>> content = () => subject.GetStringContent();
        return content.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
    }

    public static (bool success, string? errorMessage) TryGetSubjectModel<TModel>(this HttpResponseMessage subject, out TModel? model)
    {
        var (success, errorMessage) = TryGetSubjectModel(subject, out var subjectModel, typeof(TModel));
        model = (TModel?)subjectModel;
        return (success, errorMessage);
    }

    public static (bool success, string? errorMessage) TryGetSubjectModel(this HttpResponseMessage subject, out object? model, Type modelType)
    {
        var serializer = AssertionsWebConfig.Serializer;

        Func<Task<object?>> readModel = () => subject.Content.ReadAsAsync(modelType, serializer);
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

    public static string[] CollectFailuresFromAssertion<TAsserted>(
        this Action<TAsserted> assertion,
        TAsserted subject)
    {
        var failures = new List<string>();

        try
        {
            assertion(subject);
        }
        catch (ShouldAssertException ex)
        {
            failures.Add(ex.Message);
        }
        catch (Exception ex)
        {
            failures.Add(
                $"Expected to successfully verify an assertion, but the following exception occurred: {ex}"
            );
        }

        return [.. failures];
    }
}