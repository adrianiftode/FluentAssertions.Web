## FluentAssertions.Web
A FluentAssertions extension over the HttpResponseMessage to ease the Assert part
and spend less time with debugging during the Fail part.

[![Build status](https://ci.appveyor.com/api/projects/status/93qtbyftww0snl4x/branch/master?svg=true)](https://ci.appveyor.com/project/adrianiftode/fluentassertions-web/branch/master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=FluentAssertions.Web&metric=alert_status)](https://sonarcloud.io/dashboard?id=FluentAssertions.Web)

## Examples
```csharp
[Fact]
public async Task Post_ReturnsOk()
{
	// Arrange
	var client = _factory.CreateClient();

	// Act
	var response = await client.PostAsync("/api/comments", new StringContent(@"{
			  ""author"": ""John"",
			  ""content"": ""Hey, you...""
			}", Encoding.UTF8, "application/json"));

	// Assert
	response.Should().Be200Ok();
}
```

![FailedTest1](https://github.com/adrianiftode/FluentAssertions.Web/blob/master/docs/images/FailedTest1.png?raw=true)

```csharp
[Fact]
public async Task Post_WithNoAuthor_ReturnsBadRequestWithUsefulMessage()
{
	// Arrange
	var client = _factory.CreateClient();

	// Act
	var response = await client.PostAsync("/api/comments", new StringContent(@"{
								  ""content"": ""Hey, you...""
								}", Encoding.UTF8, "application/json"));

	// Assert
	response.Should().Be400BadRequest()
		.And.HaveError("Author", "The Author field is required.");
}
```
	
![FailedTest2](https://github.com/adrianiftode/FluentAssertions.Web/blob/master/docs/images/FailedTest2.png?raw=true)

## Full API

|  *HttpResponseMessageAssertions* | Contains a number of methods to assert that an  is in the expected state related to the HTTP content. | 
| --- | --- |
| **Should().Be1XXInformational()**  |  Asserts that a HTTP response has a HTTP status code representing an informational response.  | 
| **Should().Be2XXSuccessful()**  | Asserts that a HTTP response has a successful HTTP status code.  | 
| **Should().Be4XXClientError()**  | Asserts that a HTTP response has a HTTP status code representing a client error.  | 
| **Should().Be3XXRedirection()**  | Asserts that a HTTP response has a HTTP status code representing a redirection response.  | 
| **Should().Be5XXServerError()**  | Asserts that a HTTP response has a HTTP status code representing a server error.  | 
| **Should().Be100Continue()**  | Asserts that a HTTP response has the HTTP status 100 Continue  | 
| **Should().Be101SwitchingProtocols()**  | Asserts that a HTTP response has the HTTP status 101 Switching Protocols  | 
| **Should().Be200Ok()**  | Asserts that a HTTP response has the HTTP status 200 Ok  | 
| **Should().Be201Created()**  | Asserts that a HTTP response has the HTTP status 201 Created  | 
| **Should().Be202Accepted()**  | Asserts that a HTTP response has the HTTP status 202 Accepted  | 
| **Should().Be203NonAuthoritativeInformation()**  | Asserts that a HTTP response has the HTTP status 203 Non Authoritative Information  | 
| **Should().Be204NoContent()**  | Asserts that a HTTP response has the HTTP status 204 No Content  | 
| **Should().Be205ResetContent()**  | Asserts that a HTTP response has the HTTP status 205 Reset Content  | 
| **Should().Be206PartialContent()**  | Asserts that a HTTP response has the HTTP status 206 Partial Content  | 
| **Should().Be300Ambiguous()**  | Asserts that a HTTP response has the HTTP status 300 Ambiguous  | 
| **Should().Be300MultipleChoices()**  | Asserts that a HTTP response has the HTTP status 300 Multiple Choices  | 
| **Should().Be301Moved()**  | Asserts that a HTTP response has the HTTP status 301 Moved Permanently  | 
| **Should().Be301MovedPermanently()**  | Asserts that a HTTP response has the HTTP status 301 Moved Permanently  | 
| **Should().Be302Found()**  | Asserts that a HTTP response has the HTTP status 302 Found  | 
| **Should().Be302Redirect()**  | Asserts that a HTTP response has the HTTP status 302 Redirect  | 
| **Should().Be303RedirectMethod()**  | Asserts that a HTTP response has the HTTP status 303 Redirect Method  | 
| **Should().Be303SeeOther()**  | Asserts that a HTTP response has the HTTP status 303 See Other  | 
| **Should().Be304NotModified()**  | Asserts that a HTTP response has the HTTP status 304 Not Modified  | 
| **Should().Be305UseProxy()**  | Asserts that a HTTP response has the HTTP status 305 Use Proxy  | 
| **Should().Be306Unused()**  | Asserts that a HTTP response has the HTTP status 306 Unused  | 
| **Should().Be307RedirectKeepVerb()**  | Asserts that a HTTP response has the HTTP status 307 Redirect Keep Verb  | 
| **Should().Be307TemporaryRedirect()**  | Asserts that a HTTP response has the HTTP status 307 Temporary Redirect  | 
| **Should().Be400BadRequest()**  | Asserts that a HTTP response has the HTTP status 400 BadRequest  | 
| **Should().Be401Unauthorized()**  | Asserts that a HTTP response has the HTTP status 401 Unauthorized  | 
| **Should().Be402PaymentRequired()**  | Asserts that a HTTP response has the HTTP status 402 Payment Required  | 
| **Should().Be403Forbidden()**  | Asserts that a HTTP response has the HTTP status 403 Forbidden  | 
| **Should().Be404NotFound()**  | Asserts that a HTTP response has the HTTP status 404 Not Found  | 
| **Should().Be405MethodNotAllowed()**  | Asserts that a HTTP response has the HTTP status 405 Method Not Allowed  | 
| **Should().Be406NotAcceptable()**  | Asserts that a HTTP response has the HTTP status 406 Not Acceptable  | 
| **Should().Be407ProxyAuthenticationRequired()**  | Asserts that a HTTP response has the HTTP status 407 Proxy Authentication Required  | 
| **Should().Be408RequestTimeout()**  | Asserts that a HTTP response has the HTTP status 408 Request Timeout  | 
| **Should().Be409Conflict()**  | Asserts that a HTTP response has the HTTP status 409 Conflict  | 
| **Should().Be410Gone()**  | Asserts that a HTTP response has the HTTP status 410 Gone  | 
| **Should().Be411LengthRequired()**  | Asserts that a HTTP response has the HTTP status 411 Length Required  | 
| **Should().Be412PreconditionFailed()**  | Asserts that a HTTP response has the HTTP status 412 Precondition Failed  | 
| **Should().Be413RequestEntityTooLarge()**  | Asserts that a HTTP response has the HTTP status 413 Request Entity Too Large  | 
| **Should().Be414RequestUriTooLong()**  | Asserts that a HTTP response has the HTTP status 414 Request Uri Too Long  | 
| **Should().Be415UnsupportedMediaType()**  | Asserts that a HTTP response has the HTTP status 415 Unsupported Media Type  | 
| **Should().Be416RequestedRangeNotSatisfiable()**  | Asserts that a HTTP response has the HTTP status 416 Requested Range Not Satisfiable  | 
| **Should().Be417ExpectationFailed()**  | Asserts that a HTTP response has the HTTP status 417 Expectation Failed  | 
| **Should().Be426UpgradeRequired()**  | Asserts that a HTTP response has the HTTP status 426 UpgradeRequired  | 
| **Should().Be500InternalServerError()**  | Asserts that a HTTP response has the HTTP status 500 Internal Server Error  | 
| **Should().Be501NotImplemented()**  | Asserts that a HTTP response has the HTTP status 501 Not Implemented  | 
| **Should().Be502BadGateway()**  | Asserts that a HTTP response has the HTTP status 502 Bad Gateway  | 
| **Should().Be503ServiceUnavailable()**  | Asserts that a HTTP response has the HTTP status 503 Service Unavailable  | 
| **Should().Be504GatewayTimeout()**  | Asserts that a HTTP response has the HTTP status 504 Gateway Timeout  | 
| **Should().Be505HttpVersionNotSupported()**  | Asserts that a HTTP response has the HTTP status 505 Http Version Not Supported  | 
| **Should().BeAs()**  | Asserts that HTTP response content can be an equivalent representation of the expected model.  | 
| **Should().HaveHeader()**  | Asserts that an HTTP response has a named header.  | 
| **Should().NotHaveHeader()**  | Asserts that an HTTP response does not have a named header.  | 
| **Should().HaveHttpStatus()**  | Asserts that a HTTP response has a HTTP status with the specified code.  | 
| **Should().NotHaveHttpStatus()**  |  that a HTTP response does not have a HTTP status with the specified code.  | 
| **Should().MatchInContent()**  | Asserts that HTTP response has content that matches a wildcard pattern.  | 
| **Should().Satisfy()**  |  Asserts that an HTTP response satisfies a condition about it.  | 

|  *Should().HaveHeader().And.* | Contains a number of methods to assert that an  is in the expected state related to HTTP headers. | 
| --- | --- | 
| **BeEmpty()**  | Asserts that an existing HTTP header in a HTTP response has no values.  | 
| **BeValues()**  | Asserts that an existing HTTP header in a HTTP response has an expected list of header values.  | 
| **Match()**  | Asserts that an existing HTTP header in a HTTP response contains at least a value that matches a wildcard pattern.  | 

|  *Should().Be400BadRequest().And.* | Contains a number of methods to assert that an  is in the expected state related to HTTP Bad Request response | 
| --- | --- | 
| **HaveError()**  | Asserts that a Bad Request HTTP response content contains an error message identifiable by an expected field name and a wildcard error text.  | 
| **OnlyHaveError()**  | Asserts that a Bad Request HTTP response content contains only a single error message identifiable by an expected field name and a wildcard error text.  | 
| **NotHaveError()**  | Asserts that a Bad Request HTTP response content does not contain an error message identifiable by an expected field name and a wildcard error text.  | 
| **HaveErrorMessage()**  | Asserts that a Bad Request HTTP response content contains an error message identifiable by an wildcard error text.  | 
