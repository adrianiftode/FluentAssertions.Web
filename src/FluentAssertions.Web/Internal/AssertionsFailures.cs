#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

internal class AssertionsFailures
{
    public AssertionsFailures(string[] failuresMessages)
    {
        FailuresMessages = failuresMessages;
    }
    public IReadOnlyCollection<string> FailuresMessages { get; }
}
