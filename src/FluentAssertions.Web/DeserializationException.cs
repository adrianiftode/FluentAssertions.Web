// ReSharper disable once CheckNamespace
using System;
using System.Runtime.Serialization;

namespace FluentAssertions;

/// <summary>
/// Captures serialization exceptions.
/// </summary>
[Serializable]
public class DeserializationException : Exception
{
    /// <summary>
    /// Argless constructor used to created new instances of this class.
    /// </summary>
    public DeserializationException()
    {
    }

    /// <summary>
    /// Constructor used to created new instances of this class given a certain message.
    /// </summary>
    /// <param name="message">The exception message.</param>

    public DeserializationException(string message) : base(message)
    {
    }

    /// <summary>
    ///  Constructor used to created new instances of this class given a certain message and an exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">Usually the caught inner exception.</param>

    public DeserializationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///  Specialized constructor used to deserialize eventually serialized exceptions of this type.
    /// </summary>
    /// <param name="info">Serialization info.</param>
    /// <param name="context">Streaming context</param>

    protected DeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
