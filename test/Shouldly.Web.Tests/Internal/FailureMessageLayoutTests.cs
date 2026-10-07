using System.Net;

namespace Shouldly.Web.Tests.Internal;

/// <summary>
/// Golden-output tests for every failure-message layout Task 05 introduces. Each test pins the
/// exact message text in a raw string literal. If a Shouldly upgrade changes the layout, these
/// are the tests that should fail, and the only ones. The HTTP dump is cut to its first line
/// before comparing.
/// </summary>
public class FailureMessageLayoutTests
{
    private const string DumpMarker = "The HTTP response was:";

    [Fact]
    public void Exact_code_with_customMessage()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.OK);

        var message = Capture(() => response.ShouldBe201Created("we need it"));

        message.ShouldBe(Normalise("""
            response.StatusCode
                should be
            HttpStatusCode.Created
                but was
            HttpStatusCode.OK

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    [Fact]
    public void Exact_code_without_customMessage()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.OK);

        var message = Capture(() => response.ShouldBe201Created());

        message.ShouldBe(Normalise("""
            response.StatusCode
                should be
            HttpStatusCode.Created
                but was
            HttpStatusCode.OK

            The HTTP response was:
            """));
    }

    [Fact]
    public void A_family_expectation_renders_as_prose()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var message = Capture(() => response.ShouldBe2XXSuccessful("we need it"));

        message.ShouldBe(Normalise("""
            response.StatusCode
                should be
            a 2XX successful status code
                but was
            HttpStatusCode.NotFound

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    [Fact]
    public void An_actual_code_missing_from_the_enum_renders_with_its_numeric_value()
    {
        using var response = new HttpResponseMessage((HttpStatusCode)418);

        var message = Capture(() => response.ShouldBe200Ok("we need it"));

        message.ShouldBe(Normalise("""
            response.StatusCode
                should be
            HttpStatusCode.OK
                but was
            HttpStatusCode.418

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    [Fact]
    public void A_null_subject_fails_before_the_dump()
    {
        using var response = (HttpResponseMessage?)null;

        var message = Capture(() => response.ShouldBe201Created("we need it"));

        message.ShouldBe(Normalise("""
            response
                should not be null but was

            Additional Info:
                we need it
            """));
    }

    // Shouldly's ShouldNotBe renders no actual value after "but was" in 5.0.0-preview.2; the
    // empty line is part of the pinned layout.
    [Fact]
    public void ShouldNotHaveHttpStatusCode_uses_the_should_not_be_layout()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.OK);

        var message = Capture(() => response.ShouldNotHaveHttpStatusCode(HttpStatusCode.OK, "we need it"));

        message.ShouldBe(Normalise("""
            response.StatusCode
                should not be
            HttpStatusCode.OK
                but was

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    [Fact]
    public void ShouldBeEmpty_uses_the_should_be_null_or_empty_layout()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("\"Hey\"")
        };

        var message = Capture(() => response.ShouldBeEmpty("we need it"));

        message.ShouldBe(Normalise("""
            response.Content (""Hey"")
                should be null or empty

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    private sealed record CommentModel(string Comment);

    [Fact]
    public void ShouldBeAs_failure_to_deserialize_uses_the_should_be_as_layout()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("// not JSON")
        };

        var message = Capture(() => response.ShouldBeAs(new CommentModel("Hey"), "we need it"));

        message.ShouldBe(Normalise("""
            response.Content
                should be as
            Shouldly.Web.Tests.Internal.FailureMessageLayoutTests+CommentModel
                but was
            "Exception while deserializing the model with SystemTextJsonSerializer: '/' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0."

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    [Fact]
    public void ShouldBeAs_with_a_named_tuple_fails_with_the_runtime_type_guard_message()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("{ \"Property\": \"Value\", \"Other\": null }")
        };

        var message = Capture(() =>
            response.ShouldBeAs((Property: "Value", Other: (object?)null), "we need it"));

        message.ShouldContain("exist only at compile time and are absent from the runtime type");
        message.ShouldContain("we need it");
        message.ShouldContain(DumpMarker);
    }

    [Fact]
    public void ShouldMatchInContent_failure_uses_the_should_match_in_content_layout()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("""{ "author": "John", "comment": "Hey" }""")
        };

        var message = Capture(() => response.ShouldMatchInContent("*notes*", "we need it"));

        message.ShouldBe(Normalise("""
            response.Content
                should match in content
            "*notes*"
                but was
            "{ "author": "John", "comment": "Hey" }"

            Additional Info:
                we need it

            The HTTP response was:
            """));
    }

    private static string Capture(Action act) =>
        Normalise(CutDump(Should.Throw<ShouldAssertException>(act).Message));

    private static string CutDump(string message)
    {
        var markerIndex = message.IndexOf(DumpMarker, StringComparison.Ordinal);
        return markerIndex < 0 ? message : message.Substring(0, markerIndex + DumpMarker.Length);
    }

    private static string Normalise(string text) => text.Replace("\r\n", "\n").Replace("\r", "\n");
}
