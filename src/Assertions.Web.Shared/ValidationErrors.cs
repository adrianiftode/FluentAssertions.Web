namespace Assertions.Web.Internal;

/// <summary>
/// Reads validation-error data from a Bad Request HTTP response, mirroring the semantics of the
/// FluentAssertions.Web <c>BadRequestAssertions</c>: errors are read from an <c>errors</c> object
/// when present, otherwise from the root object.
/// </summary>
internal sealed class ValidationErrors : IDisposable
{
    internal const string ErrorsPropertyName = "errors";

    private readonly JsonDocument _json;
    private readonly bool _hasErrorsProperty;
    private readonly JsonProperty _errorsProperty;

    private ValidationErrors(JsonDocument json)
    {
        _json = json;
        var errorsProperties = json.GetPropertiesByName(ErrorsPropertyName).ToList();
        _hasErrorsProperty = errorsProperties.Any();
        _errorsProperty = errorsProperties.FirstOrDefault();
        Fields = (_hasErrorsProperty
                ? json.GetChildrenNames(ErrorsPropertyName)
                : json.GetChildrenNames(""))
            .ToArray();
        AllMessages = (_hasErrorsProperty
                ? json.GetChildrenNames(ErrorsPropertyName).SelectMany(field => _errorsProperty.GetStringValuesOf(field))
                : json.GetChildrenNames("").SelectMany(field => json.GetStringValuesOf(field)))
            .ToArray();
    }

    /// <summary>The error fields, taken from the <c>errors</c> object when present, else from the root object.</summary>
    public IReadOnlyList<string> Fields { get; }

    /// <summary>Every error message of every field, for the message-only assertion.</summary>
    public IReadOnlyList<string> AllMessages { get; }

    public static ValidationErrors Read(HttpResponseMessage response)
    {
        Func<Task<JsonDocument>> jsonFunc = () => response.GetJsonDocument();
        var json = jsonFunc.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        return new ValidationErrors(json);
    }

    public bool HasField(string field)
        => Fields.Any(fieldName => string.Equals(fieldName, field, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<string> MessagesOf(string field)
        => (_hasErrorsProperty ? _errorsProperty.GetStringValuesOf(field) : _json.GetStringValuesOf(field)).ToArray();

    public IReadOnlyList<string> SiblingsOf(string field)
        => _json.GetChildrenNames(_hasErrorsProperty ? ErrorsPropertyName : _json.GetParentKey(field)).ToArray();

    public void Dispose() => _json.Dispose();
}