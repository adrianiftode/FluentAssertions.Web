using HttpMessageFormatter.Internal;

namespace HttpMessageFormatter;

/// <summary>
/// Parameter object with the options used when formatting an <see cref="HttpResponseMessage"/>.
/// </summary>
public class HttpResponseFormatterOptions
{
    private int _maximumReadableBytes = ContentFormatterOptions.MaximumReadableBytes;

    /// <summary>
    /// The maximum number of characters read from the content and printed into the formatted output.
    /// When the content is longer, only a part of it is printed together with a warning message.
    /// Defaults to 10 * 128 * 1024 (1,310,720) characters.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The value is less than one.</exception>
    public int MaximumReadableBytes
    {
        get => _maximumReadableBytes;

        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(MaximumReadableBytes)} must be greater than zero.");
            }

            _maximumReadableBytes = value;
        }
    }
}
