## FluentAssertions.Web
A FluentAssertions extension over the HttpResponseMessage to ease the Assert part
and spend less time with debugging during the Fail part.

[![Build status](https://ci.appveyor.com/api/projects/status/93qtbyftww0snl4x/branch/master?svg=true)](https://ci.appveyor.com/project/adrianiftode/fluentassertions-web/branch/master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=adrianiftode_FluentAssertions.Web&metric=alert_status)](https://sonarcloud.io/dashboard?id=adrianiftode_FluentAssertions.Web)

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

|  *HttpResponseMessageAssertions* | Contains a number of methods to assert that an  is in the expected state related to the HTTP content. | 
| --- | --- | 
| **Be200Ok**  | Asserts that a HTTP response has the HTTP status 200 Ok  | 
| **Be201Created**  | Asserts that a HTTP response has the HTTP status 201 Created  | 
| **Be202Accepted**  | Asserts that a HTTP response has the HTTP status 202 Accepted  | 
| **Be203NonAuthoritativeInformation**  | Asserts that a HTTP response has the HTTP status 203 Non Authoritative Information  | 
| **Be204NoContent**  | Asserts that a HTTP response has the HTTP status 204 No Content  | 
| **Be205ResetContent**  | Asserts that a HTTP response has the HTTP status 205 Reset Content  | 
| **Be206PartialContent**  | Asserts that a HTTP response has the HTTP status 206 Partial Content  | 
| **Be300Ambiguous**  | Asserts that a HTTP response has the HTTP status 300 Ambiguous  | 
| **Be300MultipleChoices**  | Asserts that a HTTP response has the HTTP status 300 Multiple Choices  | 
| **Be301Moved**  | Asserts that a HTTP response has the HTTP status 301 Moved Permanently  | 
| **Be301MovedPermanently**  | Asserts that a HTTP response has the HTTP status 301 Moved Permanently  | 
| **Be302Found**  | Asserts that a HTTP response has the HTTP status 302 Found  | 
| **Be302Redirect**  | Asserts that a HTTP response has the HTTP status 302 Redirect  | 
| **Be303RedirectMethod**  | Asserts that a HTTP response has the HTTP status 303 Redirect Method  | 
| **Be303SeeOther**  | Asserts that a HTTP response has the HTTP status 303 See Other  | 
| **Be304NotModified**  | Asserts that a HTTP response has the HTTP status 304 Not Modified  | 
| **Be305UseProxy**  | Asserts that a HTTP response has the HTTP status 305 Use Proxy  | 
| **Be306Unused**  | Asserts that a HTTP response has the HTTP status 306 Unused  | 
| **Be307RedirectKeepVerb**  | Asserts that a HTTP response has the HTTP status 307 Redirect Keep Verb  | 
| **Be307TemporaryRedirect**  | Asserts that a HTTP response has the HTTP status 307 Temporary Redirect  | 
| **Be400BadRequest**  | Asserts that a HTTP response has the HTTP status 400 BadRequest  | 
| **Be401Unauthorized**  | Asserts that a HTTP response has the HTTP status 401 Unauthorized  | 
| **Be402PaymentRequired**  | Asserts that a HTTP response has the HTTP status 402 Payment Required  | 
| **Be403Forbidden**  | Asserts that a HTTP response has the HTTP status 403 Forbidden  | 
| **Be404NotFound**  | Asserts that a HTTP response has the HTTP status 404 Not Found  | 
| **Be405MethodNotAllowed**  | Asserts that a HTTP response has the HTTP status 405 Method Not Allowed  | 
| **Be406NotAcceptable**  | Asserts that a HTTP response has the HTTP status 406 Not Acceptable  | 
| **Be407ProxyAuthenticationRequired**  | Asserts that a HTTP response has the HTTP status 407 Proxy Authentication Required  | 
| **Be408RequestTimeout**  | Asserts that a HTTP response has the HTTP status 408 Request Timeout  | 
| **Be409Conflict**  | Asserts that a HTTP response has the HTTP status 409 Conflict  | 
| **Be410Gone**  | Asserts that a HTTP response has the HTTP status 410 Gone  | 
| **Be411LengthRequired**  | Asserts that a HTTP response has the HTTP status 411 Length Required  | 
| **Be412PreconditionFailed**  | Asserts that a HTTP response has the HTTP status 412 Precondition Failed  | 
| **Be413RequestEntityTooLarge**  | Asserts that a HTTP response has the HTTP status 413 Request Entity Too Large  | 
| **Be414RequestUriTooLong**  | Asserts that a HTTP response has the HTTP status 414 Request Uri Too Long  | 
| **Be415UnsupportedMediaType**  | Asserts that a HTTP response has the HTTP status 415 Unsupported Media Type  | 
| **Be416RequestedRangeNotSatisfiable**  | Asserts that a HTTP response has the HTTP status 416 Requested Range Not Satisfiable  | 
| **Be417ExpectationFailed**  | Asserts that a HTTP response has the HTTP status 417 Expectation Failed  | 
| **Be426UpgradeRequired**  | Asserts that a HTTP response has the HTTP status 426 UpgradeRequired  | 
| **Be500InternalServerError**  | Asserts that a HTTP response has the HTTP status 500 Internal Server Error  | 
| **Be501NotImplemented**  | Asserts that a HTTP response has the HTTP status 501 Not Implemented  | 
| **Be502BadGateway**  | Asserts that a HTTP response has the HTTP status 502 Bad Gateway  | 
| **Be503ServiceUnavailable**  | Asserts that a HTTP response has the HTTP status 503 Service Unavailable  | 
| **Be504GatewayTimeout**  | Asserts that a HTTP response has the HTTP status 504 Gateway Timeout  | 
| **Be505HttpVersionNotSupported**  | Asserts that a HTTP response has the HTTP status 505 Http Version Not Supported  | 
| **HaveContent**  | Asserts that HTTP response content can be an equivalent representation of the expected model.  | 
| **HaveHeader**  | Asserts that an HTTP response has a named header.  | 

 
|  *HeadersAssertions* | Contains a number of methods to assert that an  is in the expected state related to HTTP headers. | 
| --- | --- | 
| **BeEmpty**  | Asserts that an existing HTTP header in a HTTP response has no values.  | 
| **BeValues**  | Asserts that an existing HTTP header in a HTTP response has an expected list of header values.  | 
| **Match**  | Asserts that an existing HTTP header in a HTTP response contains at least a value that matches a wildcard pattern.  | 

|  *BadRequestAssertions* | Contains a number of methods to assert that an  is in the expected state related to HTTP Bad Request response | 
| --- | --- | 
| **HaveError**  | Asserts that a Bad Request HTTP response content contains an error message identifiable by an expected field name and a wildcard error text.  | 
| **HaveErrorMessage**  | Asserts that a Bad Request HTTP response content contains an error message identifiable by an wildcard error text.  | 
