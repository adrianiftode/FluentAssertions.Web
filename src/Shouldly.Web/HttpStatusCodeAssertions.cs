// ReSharper disable CheckNamespace
using System.Xml.Linq;

namespace Shouldly;

/// <summary>
/// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state related to HTTP Bad Request actual
/// </summary>
[ShouldlyMethods]
public static class HttpStatusCodeAssertions
{
    #region Be1XXInformational
    /// <summary>
    /// Asserts that an HTTP actual has an HTTP status code representing an informational actual.
    /// </summary>
    /// <remarks>The HTTP actual was an informational one if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 100-199.</remarks>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    // ReSharper disable once InconsistentNaming
    public static void ShouldBe1XXInformational(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(actual!.StatusCode < HttpStatusCode.OK, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have an HTTP status code representing an informational error, but it was {0}{reason}.{1}",
                actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region Be2XXSuccessful
    /// <summary>
    /// Asserts that an HTTP actual has a successful HTTP status code.
    /// </summary>
    /// <remarks>The HTTP actual was successful if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 200-299.</remarks>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    // ReSharper disable once InconsistentNaming
    public static void ShouldBe2XXSuccessful(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(actual!.IsSuccessStatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have a successful HTTP status code, but it was {0}{reason}.{1}",
                actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region Be3XXRedirection
    /// <summary>
    /// Asserts that an HTTP actual has an HTTP status code representing a redirection actual.
    /// </summary>
    /// <remarks>The HTTP actual was a redirection one if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 300-399.</remarks>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    // ReSharper disable once InconsistentNaming
    public static void ShouldBe3XXRedirection(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(actual!.StatusCode >= HttpStatusCode.Moved && actual!.StatusCode < HttpStatusCode.BadRequest, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have an HTTP status code representing a redirection, but it was {0}{reason}.{1}",
                actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region Be4XXClientError
    /// <summary>
    /// Asserts that an HTTP actual has an HTTP status code representing a client error.
    /// </summary>
    /// <remarks>The HTTP actual was a client error if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 400-499.</remarks>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    // ReSharper disable once InconsistentNaming
    public static void ShouldBe4XXClientError(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(actual!.StatusCode >= HttpStatusCode.BadRequest && actual!.StatusCode < HttpStatusCode.InternalServerError, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have an HTTP status code representing a client error, but it was {0}{reason}.{1}",
                actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region Be5XXServerError
    /// <summary>
    /// Asserts that an HTTP actual has an HTTP status code representing a server error.
    /// </summary>
    /// <remarks>The HTTP actual was a server error if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was above 500.</remarks>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    // ReSharper disable once InconsistentNaming
    public static void ShouldBe5XXServerError(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(actual!.StatusCode >= HttpStatusCode.InternalServerError, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to have an HTTP status code representing a server error, but it was {0}{reason}.{1}",
                actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region HaveHttpStatus
    /// <summary>
    /// Asserts that an HTTP actual has an HTTP status with the specified code.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="expected">
    /// The code of the expected HTTP Status.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldHaveHttpStatusCode(this HttpResponseMessage? actual, HttpStatusCode expected, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(expected == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                expected, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region NotHaveHttpStatus
    /// <summary>
    /// Asserts that an HTTP actual does not have an HTTP status with the specified code.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="unexpected">
    /// The code of the unexpected HTTP Status.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldNotHaveHttpStatusCode(this HttpResponseMessage? actual, HttpStatusCode unexpected, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(unexpected != actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect {context:actual} to have status {0}{reason}.{1}",
                actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion

    #region BeXXXHttpStatus
    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 100 Continue.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe100Continue(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Continue == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.Continue, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 101 Switching Protocols.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe101SwitchingProtocols(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.SwitchingProtocols == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.SwitchingProtocols, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 200 Ok.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe200Ok(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.OK == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.OK, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 201 Created.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe201Created(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Created == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.Created, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 202 Accepted.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe202Accepted(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Accepted == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.Accepted, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 203 Non Authoritative Information.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe203NonAuthoritativeInformation(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.NonAuthoritativeInformation == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.NonAuthoritativeInformation, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 204 No Content.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe204NoContent(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.NoContent == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.NoContent, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 205 Reset Content.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe205ResetContent(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.ResetContent == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.ResetContent, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 206 Partial Content.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe206PartialContent(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.PartialContent == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.PartialContent, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 300 Multiple Choices.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe300MultipleChoices(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.MultipleChoices == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                "HttpStatusCode.MultipleChoices {value: 300}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 300 Ambiguous.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe300Ambiguous(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Ambiguous == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                $"{nameof(HttpStatusCode)}.{nameof(HttpStatusCode.Ambiguous)}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 301 Moved Permanently.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe301MovedPermanently(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.MovedPermanently == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                "HttpStatusCode.MovedPermanently {value: 301}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 301 Moved.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe301Moved(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Moved == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.Moved, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 302 Found.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe302Found(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Found == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                "HttpStatusCode.Found {value: 302}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 302 Redirect.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe302Redirect(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Redirect == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                $"{nameof(HttpStatusCode)}.{nameof(HttpStatusCode.Redirect)}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 303 See Other.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe303SeeOther(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.SeeOther == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                "HttpStatusCode.SeeOther {value: 303}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 303 Redirect Method.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe303RedirectMethod(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.RedirectMethod == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                $"{nameof(HttpStatusCode)}.{nameof(HttpStatusCode.RedirectMethod)}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 304 Not Modified.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe304NotModified(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.NotModified == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.NotModified, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 305 Use Proxy.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe305UseProxy(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.UseProxy == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.UseProxy, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 306 Unused.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe306Unused(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Unused == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.Unused, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 307 Temporary Redirect.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe307TemporaryRedirect(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.TemporaryRedirect == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                $"{nameof(HttpStatusCode)}.{nameof(HttpStatusCode.TemporaryRedirect)}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 307 Redirect Keep Verb.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe307RedirectKeepVerb(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.RedirectKeepVerb == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                "HttpStatusCode.RedirectKeepVerb {value: 307}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 308 Permanent Redirect.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe308PermanentRedirect(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(308 == (int)actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                "HttpStatusCode.PermanentRedirect {value: 308}", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 400 BadRequest.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe400BadRequest(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.BadRequest == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.BadRequest, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 401 Unauthorized.
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe401Unauthorized(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Unauthorized == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}",
                HttpStatusCode.Unauthorized, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 402 Payment Required
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe402PaymentRequired(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.PaymentRequired == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.PaymentRequired, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 403 Forbidden
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe403Forbidden(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Forbidden == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.Forbidden, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 404 Not Found
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe404NotFound(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.NotFound == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.NotFound, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 405 Method Not Allowed
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe405MethodNotAllowed(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.MethodNotAllowed == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.MethodNotAllowed, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 406 Not Acceptable
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe406NotAcceptable(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.NotAcceptable == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.NotAcceptable, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 407 Proxy Authentication Required
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe407ProxyAuthenticationRequired(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.ProxyAuthenticationRequired == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.ProxyAuthenticationRequired, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 408 Request Timeout
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe408RequestTimeout(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.RequestTimeout == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.RequestTimeout, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 409 Conflict
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe409Conflict(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Conflict == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.Conflict, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 410 Gone
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe410Gone(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.Gone == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.Gone, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 411 Length Required
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe411LengthRequired(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.LengthRequired == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.LengthRequired, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 412 Precondition Failed
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe412PreconditionFailed(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.PreconditionFailed == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.PreconditionFailed, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 413 Request Entity Too Large
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe413RequestEntityTooLarge(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.RequestEntityTooLarge == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.RequestEntityTooLarge, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 414 Request Uri Too Long
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe414RequestUriTooLong(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.RequestUriTooLong == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.RequestUriTooLong, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 415 Unsupported Media Type
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe415UnsupportedMediaType(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.UnsupportedMediaType == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.UnsupportedMediaType, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 416 Requested Range Not Satisfiable
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe416RequestedRangeNotSatisfiable(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.RequestedRangeNotSatisfiable == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.RequestedRangeNotSatisfiable, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 417 Expectation Failed
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe417ExpectationFailed(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.ExpectationFailed == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.ExpectationFailed, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 422 UnprocessableEntity
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe422UnprocessableEntity(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(422 == (int)actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , "HttpStatusCode.UnprocessableEntity", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 429 TooManyRequests
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe429TooManyRequests(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(429 == (int)actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , "HttpStatusCode.TooManyRequests", actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 426 UpgradeRequired
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe426UpgradeRequired(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.UpgradeRequired == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.UpgradeRequired, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 500 Internal Server Error
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe500InternalServerError(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.InternalServerError == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.InternalServerError, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 501 Not Implemented
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe501NotImplemented(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.NotImplemented == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.NotImplemented, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 502 Bad Gateway
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe502BadGateway(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.BadGateway == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.BadGateway, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 503 Service Unavailable
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe503ServiceUnavailable(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.ServiceUnavailable == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.ServiceUnavailable, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 504 Gateway Timeout
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe504GatewayTimeout(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.GatewayTimeout == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.GatewayTimeout, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }

    /// <summary>
    /// Asserts that an HTTP actual has the HTTP status 505 Http Version Not Supported
    /// </summary>
    /// <param name="actual">The actual HttpResponseMessage to be asserted on.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    public static void ShouldBe505HttpVersionNotSupported(this HttpResponseMessage? actual, string because = "", params object[] becauseArgs)
    {
        ExecuteAssertion
            .ForCondition(actual is not null, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected a {context:actual} to assert{reason}, but found <null>.");

        ExecuteAssertion
            .ForCondition(HttpStatusCode.HttpVersionNotSupported == actual!.StatusCode, nameof(actual))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:actual} to be {0}{reason}, but found {1}.{2}"
                , HttpStatusCode.HttpVersionNotSupported, actual!.StatusCode, new FormatHttpResponseMessage(actual));
    }
    #endregion
}