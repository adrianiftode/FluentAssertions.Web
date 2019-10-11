using System.Collections.Generic;

namespace FluentAssertions.Web.Internal
{
    public class AssertionsFailures
    {
        public AssertionsFailures(string[] failuresMessages)
        {
            FailuresMessages = failuresMessages;
        }
        public IReadOnlyCollection<string> FailuresMessages { get; }
    }
}
