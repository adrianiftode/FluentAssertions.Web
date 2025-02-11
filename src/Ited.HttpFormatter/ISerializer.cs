namespace Ited.HttpFormatter;

/// <summary>
/// Provides an abstraction to deserialize a Stream of binary data into a C# object.
/// </summary>
public interface ISerializer
{
    /// <summary>
    ///  Deserialize a Stream of binary data into a C# object of a specific model.
    /// </summary>
    /// <param name="content">The stream to read data from.</param>
    /// <param name="modelType">The deserialized model's structure.</param>
    /// <returns>An object representing the model.</returns>
    Task<object?> Deserialize(Stream content, Type modelType);
}
