using System.Text.RegularExpressions;

namespace Shouldly.Web.Internal;

internal class ExecuteAssertion
{
    private readonly bool _condition;
    private readonly string _contextName;
    private string _becausePhrase = string.Empty;
    private object[] _becauseArgs = [];

    private ExecuteAssertion(bool condition, string contextName)
    {
        _condition = condition;
        _contextName = string.IsNullOrWhiteSpace(contextName) ? "actual" : contextName;
    }

    /// <summary>
    /// The single static entry point for evaluating conditions and capturing context names.
    /// </summary>
    public static ExecuteAssertion ForCondition(bool condition, string contextName = "actual")
    {
        return new ExecuteAssertion(condition, contextName);
    }

    /// <summary>
    /// Chains custom reasons and structural formatting arguments.
    /// </summary>
    public ExecuteAssertion BecauseOf(string because, params object[]? becauseArgs)
    {
        if (!string.IsNullOrWhiteSpace(because))
        {
            var trimmed = because.Trim();
            _becausePhrase = trimmed.StartsWith("because", StringComparison.OrdinalIgnoreCase)
                ? $" {trimmed}"
                : $" because {trimmed}";

            _becauseArgs = becauseArgs ?? [];
        }
        return this;
    }

    /// <summary>
    /// Evaluates the assertion. Throws formatted multiline exceptions on failure.
    /// </summary>
    public void FailWith(string messageTemplate, params object[]? messageArgs)
    {
        if (_condition) return;

        var formattedReason = _becausePhrase;
        if (_becauseArgs.Length > 0 && !string.IsNullOrEmpty(_becausePhrase))
        {
            try
            {
                formattedReason = string.Format(_becausePhrase, _becauseArgs);
            }
            catch (FormatException) { }
        }

        var processedTemplate = messageTemplate
            .Replace("{context:actual}", _contextName)
            .Replace("{reason}", formattedReason);

        var finalMessage = messageArgs is { Length: > 0 }
            ? string.Format(processedTemplate, messageArgs)
            : processedTemplate;

        // Strip horizontal duplicate spacing while fully preserving line breaks (\n)
        finalMessage = Regex.Replace(finalMessage, @"[\t ]+", " ").Trim();

        throw new ShouldAssertException(finalMessage);
    }
}