using System;
using System.Collections.Generic;
using System.Text;

namespace Shouldly.Web.Internal;

internal static class ShouldAssertExceptionExtensions
{
    public static AssertionsFailure ExtractAssertionFailure(this ShouldAssertException exception)
    {
        var indexOfError1 = exception.Message?.IndexOf("---------------- Error 1 ----------------") ?? -1;
        var stripped = indexOfError1 > -1 ? exception.Message?.Substring(indexOfError1) : null;

        return new AssertionsFailure(stripped);
    }
}