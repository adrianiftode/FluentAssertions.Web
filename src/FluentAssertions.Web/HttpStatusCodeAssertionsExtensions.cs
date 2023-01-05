﻿// ReSharper disable once CheckNamespace
namespace FluentAssertions;

/// <summary>
/// Contains extension methods for custom assertions in unit tests related to <see cref="BadRequestAssertions"/>.
/// </summary>
public static class HttpStatusCodeAssertionsExtensions
{
    #region Be1XXInformational
    /// <summary>
    /// Asserts that a HTTP response has a HTTP status code representing an informational response.
    /// </summary>
    /// <remarks>The HTTP response was an informational one if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 100-199.</remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    // ReSharper disable once InconsistentNaming
    public static AndConstraint<HttpResponseMessageAssertions> Be1XXInformational(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be1XXInformational(because, becauseArgs);
    #endregion

    #region Be2XXSuccessful
    /// <summary>
    /// Asserts that a HTTP response has a successful HTTP status code.
    /// </summary>
    /// <remarks>The HTTP response was successful if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 200-299.</remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    // ReSharper disable once InconsistentNaming
    public static AndConstraint<HttpResponseMessageAssertions> Be2XXSuccessful(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be2XXSuccessful(because, becauseArgs);
    #endregion

    #region Be3XXRedirection
    /// <summary>
    /// Asserts that a HTTP response has a HTTP status code representing a redirection response.
    /// </summary>
    /// <remarks>The HTTP response was an informational one if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 300-399.</remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    // ReSharper disable once InconsistentNaming
    public static AndConstraint<HttpResponseMessageAssertions> Be3XXRedirection(
#pragma warning disable 1573
    this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
    string because = "", params object[] becauseArgs)
    => new HttpResponseMessageAssertions(parent.Subject).Be3XXRedirection(because, becauseArgs);
    #endregion

    #region Be4XXClientError
    /// <summary>
    /// Asserts that a HTTP response has a HTTP status code representing a client error.
    /// </summary>
    /// <remarks>The HTTP response was a client error if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 400-499.</remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    // ReSharper disable once InconsistentNaming
    public static AndConstraint<HttpResponseMessageAssertions> Be4XXClientError(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be4XXClientError(because, becauseArgs);
    #endregion

    #region Be5XXServerError
    /// <summary>
    /// Asserts that a HTTP response has a HTTP status code representing a server error.
    /// </summary>
    /// <remarks>The HTTP response was a server error if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was above 500.</remarks>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    // ReSharper disable once InconsistentNaming
    public static AndConstraint<HttpResponseMessageAssertions> Be5XXServerError(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be5XXServerError(because, becauseArgs);
    #endregion

    #region HaveHtppStatus
    /// <summary>
    /// Asserts that a HTTP response has a HTTP status with the specified code.
    /// </summary>
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
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> HaveHttpStatusCode(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        HttpStatusCode expected, string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).HaveHttpStatusCode(expected, because, becauseArgs);
    #endregion

    #region NotHaveHtppStatus
    /// <summary>
    /// Asserts that a HTTP response does not have a HTTP status with the specified code.
    /// </summary>
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
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> NotHaveHttpStatusCode(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        HttpStatusCode unexpected, string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).NotHaveHttpStatusCode(unexpected, because, becauseArgs);
    #endregion

    #region BeXXXHttpStatus
    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 100 Continue
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be100Continue(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be100Continue(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 101 Switching Protocols
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be101SwitchingProtocols(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be101SwitchingProtocols(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 200 Ok
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be200Ok(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be200Ok(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 201 Created
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be201Created(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be201Created(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 202 Accepted
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be202Accepted(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be202Accepted(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 203 Non Authoritative Information
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be203NonAuthoritativeInformation(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be203NonAuthoritativeInformation(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 204 No Content
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be204NoContent(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be204NoContent(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 205 Reset Content
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be205ResetContent(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be205ResetContent(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 206 Partial Content
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be206PartialContent(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be206PartialContent(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 300 Multiple Choices
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be300MultipleChoices(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be300MultipleChoices(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 300 Ambiguous
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be300Ambiguous(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be300Ambiguous(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 301 Moved Permanently
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HaveLocationHeaderAssertions> Be301MovedPermanently(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HaveLocationHeaderAssertions(parent.Subject).Be301MovedPermanently(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 301 Moved
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be301Moved(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be301Moved(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 302 Found
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be302Found(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be302Found(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 302 Redirect
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be302Redirect(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be302Redirect(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 303 See Other
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be303SeeOther(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be303SeeOther(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 303 Redirect Method
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be303RedirectMethod(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be303RedirectMethod(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 304 Not Modified
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be304NotModified(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be304NotModified(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 305 Use Proxy
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be305UseProxy(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be305UseProxy(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 306 Unused
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be306Unused(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be306Unused(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 307 Temporary Redirect
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be307TemporaryRedirect(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be307TemporaryRedirect(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 307 Redirect Keep Verb
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be307RedirectKeepVerb(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be307RedirectKeepVerb(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 400 BadRequest
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<BadRequestAssertions> Be400BadRequest(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new BadRequestAssertions(parent.Subject).Be400BadRequest(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 401 Unauthorized
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be401Unauthorized(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be401Unauthorized(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 402 Payment Required
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be402PaymentRequired(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be402PaymentRequired(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 403 Forbidden
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be403Forbidden(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be403Forbidden(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 404 Not Found
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be404NotFound(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be404NotFound(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 405 Method Not Allowed
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be405MethodNotAllowed(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be405MethodNotAllowed(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 406 Not Acceptable
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be406NotAcceptable(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be406NotAcceptable(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 407 Proxy Authentication Required
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be407ProxyAuthenticationRequired(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be407ProxyAuthenticationRequired(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 408 Request Timeout
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be408RequestTimeout(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be408RequestTimeout(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 409 Conflict
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be409Conflict(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be409Conflict(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 410 Gone
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be410Gone(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be410Gone(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 411 Length Required
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be411LengthRequired(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be411LengthRequired(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 412 Precondition Failed
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be412PreconditionFailed(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be412PreconditionFailed(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 413 Request Entity Too Large
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be413RequestEntityTooLarge(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be413RequestEntityTooLarge(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 414 Request Uri Too Long
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be414RequestUriTooLong(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be414RequestUriTooLong(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 415 Unsupported Media Type
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be415UnsupportedMediaType(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be415UnsupportedMediaType(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 416 Requested Range Not Satisfiable
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be416RequestedRangeNotSatisfiable(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be416RequestedRangeNotSatisfiable(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 417 Expectation Failed
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be417ExpectationFailed(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be417ExpectationFailed(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 422 UnprocessableEntity
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be422UnprocessableEntity(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be422UnprocessableEntity(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 429 TooManyRequests
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be429TooManyRequests(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be429TooManyRequests(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 426 UpgradeRequired
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be426UpgradeRequired(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be426UpgradeRequired(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 500 Internal Server Error
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be500InternalServerError(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be500InternalServerError(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 501 Not Implemented
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be501NotImplemented(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be501NotImplemented(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 502 Bad Gateway
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be502BadGateway(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be502BadGateway(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 503 Service Unavailable
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be503ServiceUnavailable(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be503ServiceUnavailable(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 504 Gateway Timeout
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be504GatewayTimeout(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be504GatewayTimeout(because, becauseArgs);

    /// <summary>
    /// Asserts that a HTTP response has the HTTP status 505 Http Version Not Supported
    /// </summary>        
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <see paramref="because" />.
    /// </param>
    [CustomAssertion]
    public static AndConstraint<HttpResponseMessageAssertions> Be505HttpVersionNotSupported(
#pragma warning disable 1573
        this Primitives.HttpResponseMessageAssertions<Primitives.HttpResponseMessageAssertions> parent,
#pragma warning restore 1573,
        string because = "", params object[] becauseArgs)
        => new HttpResponseMessageAssertions(parent.Subject).Be505HttpVersionNotSupported(because, becauseArgs);
    #endregion
}
