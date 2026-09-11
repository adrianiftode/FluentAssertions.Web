namespace Shouldly.Web.Internal;

internal class FormatHttpResponseMessage(HttpResponseMessage? httpResponseMessage)
{
    public override string ToString() => httpResponseMessage?.Format() ?? "<null>";
}

internal class FormatAssertionsFailure(AssertionsFailure? failure)
{
    public override string ToString() => failure?.Format() ?? "<null>";
}