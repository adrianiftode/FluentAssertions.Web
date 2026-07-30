

using System.Threading.Tasks;
using Assertions.Core.Internal;

// ReSharper disable CheckNamespace
namespace Shouldly;

/// <summary>
/// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state.
/// </summary>
internal static class HttpResponseMessageAssertions
{
    public static string? GetContent(HttpResponseMessage subject)
    {
        Func<Task<string?>> content = () => subject.GetStringContent();
        return content.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
    }

    public static (bool success, string? errorMessage) TryGetSubjectModel<TModel>(HttpResponseMessage subject, out TModel? model)
    {
        var (success, errorMessage) = TryGetSubjectModel(subject, out var subjectModel, typeof(TModel));
        model = (TModel?)subjectModel;
        return (success, errorMessage);
    }

    public static (bool success, string? errorMessage) TryGetSubjectModel(HttpResponseMessage subject, out object? model, Type modelType)
    {
        var serializer = ShouldlyAssertionsWebConfig.Serializer;
        
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

    //private string[] CollectFailuresFromAssertion<TAsserted>(Action<TAsserted> assertion, TAsserted subject)
    //{
    //    using var collectionScope = new AssertionScope();
    //    string[] assertionFailures;
    //    using (var itemScope = new AssertionScope())
    //    {
    //        try
    //        {
    //            assertion(subject);
    //            assertionFailures = itemScope.Discard();
    //        }
    //        catch (Exception ex)
    //        {
    //            assertionFailures = new[] { $"Expected to successfully verify an assertion, but the following exception occurred: { ex }" };
    //        }

    //    }

    //    foreach (var assertionFailure in assertionFailures)
    //    {
    //        collectionScope.AddPreFormattedFailure($"{assertionFailure}");
    //    }

    //    return collectionScope.Discard();
    //}
}