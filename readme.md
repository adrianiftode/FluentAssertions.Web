## FluentAssertions.Web
This is a [*FluentAssertions*](https://fluentassertions.com/) extension over the *HttpResponseMessage* object.

It provides assertions specific to HTTP responses and outputs rich erros messages when the tests fail, so less time with debugging is spent.

[![Build status](https://ci.appveyor.com/api/projects/status/93qtbyftww0snl4x/branch/master?svg=true)](https://ci.appveyor.com/project/adrianiftode/fluentassertions-web/branch/master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=FluentAssertions.Web&metric=alert_status)](https://sonarcloud.io/dashboard?id=FluentAssertions.Web)
[![NuGet](https://img.shields.io/nuget/v/FluentAssertions.Web.svg)](https://www.nuget.org/packages/FluentAssertions.Web)

```csharp
[Fact]
public async Task Post_ReturnsOk()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.PostAsync("/api/comments", new StringContent(
    """
    {
      "author": "John",
      "content": "Hey, you..."
    }
    """, Encoding.UTF8, "application/json"));

    // Assert
    response.Should().Be200Ok();
}
```


## Why?

Writing tests for ASP.NET Core APIs and or any APIs by using the HttpClient classes leads to some repetitive code and also often to an incomplete one. When the test fails, the developer needs to debug the test in order to assess the failure reason, as the test itself did not usually consider what happens in this case.

Thus this library solves two problems:

##### Focus on the Assert part and not on the HttpClient related APIs, neither on the response deserialization

Once the response is ready you'll want to assert it. With first level properties like `StatusCode` is somehow easy, especially with FluentAssertions, but often we need more, like to deserialize the content into an object of a certain type and then to Assert it. Or to simply assert something about the response content itself. Soon duplication code occurs and the urge to reduce it is just the next logical step. 

##### Debugging failed tests interrupts the programmer's flow state
 When a test is failing, the following actions are taken most of the time:
- attach the debugger to the line containing ```var response = await client..```
- debug the failing test
- add an Watch for ``` response.Content.ReadAsStringAsync().Result ``` and see the actual response content

**And this can be avoided**, if the *Test Detail Summary* contains the request and the response information, providing a similar experience as with an HTTP interceptor like Fiddler. 

![FailedTest1](https://github.com/adrianiftode/FluentAssertions.Web/blob/master/docs/images/FailedTest1.png?raw=true)

### FluentAssertions.Web Examples

- Asserting that the response content of a HTTP POST request is equivalent to a certain object

```csharp
[Fact]
public async Task Post_ReturnsOkAndWithContent()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.PostAsync("/api/comments", new StringContent(
    """
    {
      "author": "John",
      "content": "Hey, you..."
    }
    """, Encoding.UTF8, "application/json"));

    // Assert
    response.Should().BeAs(new
    {
        Author = "John",
        Content = "Hey, you..."
    });
}
```

- Asserting that the response is 200 OK and the content is like an array of specific objects:

```csharp
[Fact]
public async Task Get_Returns_Ok_With_CommentsList()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/comments");

    // Assert
    response.Should().Be200Ok().And.BeAs(new[]
    {
        new { Author = "Adrian", Content = "Hey" }
    });
}
```

- Asserting that the response is a HTTP 400 BadRequest and contains a single error message

```csharp
[Fact]
public async Task Post_WithNoAuthorButWithContent_ReturnsBadRequestWithAnErrorMessageRelatedToAuthorOnly()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.PostAsync("/api/comments", new StringContent(
    """
    {
      "content": "Hey, you..."
    }
    """, Encoding.UTF8, "application/json"));

    // Assert
    response.Should().Be400BadRequest()
        .And.OnlyHaveError("Author", "The Author field is required.");
}
```

- Asserting the response content once deserialized into a strongly typed object it satisfies a certain assertion

```csharp
[Fact]
public async Task Get_Returns_Ok_With_CommentsList_With_TwoUniqueComments()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/comments");

    // Assert
    response.Should().Satisfy<IEnumerable<Comment>>(model => 
            model.Should().HaveCount(2).And.OnlyHaveUniqueItems(c => c.CommentId));
}
```

- Asserting the response content once deserialized into a anonymous object it satisfies a certain assertion

```csharp
[Fact]
public async Task Get_WithCommentId_Returns_A_NonSpam_Comment()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/comments/1");

    // Assert
    response.Should().Satisfy(givenModelStructure: new
    {
        Author = default(string),
        Content = default(string)
    }, assertion: model =>
        {
            model.Author.Should().NotBe("I DO SPAM!");
            model.Content.Should().NotContain("BUY MORE");
        });
}
```

- Asserting the response has a header with the name `X-Correlation-ID` and the value matches a certain pattern

```csharp
[Fact]
public async Task Get_Should_Contain_a_Header_With_Correlation_Id()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/values");

    // Assert
    response.Should().HaveHeader("X-Correlation-ID").And.Match("*-*", "we want to test the correlation id is a Guid like one");
}
```

- Asserting the response has a header with the name `X-Correlation-ID` and the value is not empty

```csharp
[Fact]
public async Task Get_Should_Contain_a_Header_With_Correlation_Id()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync("/api/values");

    // Assert
    response.Should().HaveHeader("X-Correlation-ID").And.NotBeEmpty();
}
```

Many more examples can be found in the [Samples](https://github.com/adrianiftode/FluentAssertions.Web/tree/master/samples) projects and in the Specs files from the [FluentAssertions.Web.Tests](https://github.com/adrianiftode/FluentAssertions.Web/tree/master/test/FluentAssertions.Web.Tests) project

## Optional Global Configuration

### Deserialization

#### System.Text.Json

By default `System.Text.Json` is used to deserialize the response content. The related `System.Text.Json.JsonSerializerOptions` used to configure the serializer is accessible via the `SystemTextJsonSerializerConfig.Options` static field from FluentAssertions.Web. So if you want to make the serializer case sensitive, then the related setting is changed like this:

```csharp
SystemTextJsonSerializerConfig.Options.PropertyNameCaseInsensitive = false; 
```

The change must be done before the test is run and this depends on the testing framework. Check the NewtonsoftSerializerTests from this repo to see how it can be done with xUnit.

#### Newtonsoft.Json

The serializer itself is replaceable, so you can implement your own, by implementing the `ISerialize` interface. The serializer is shipped via the **FluentAssertions.Web.Serializers.NewtonsoftJson** package.

[![NuGet](https://img.shields.io/nuget/v/FluentAssertions.Web.Serializers.NewtonsoftJson.svg)](https://www.nuget.org/packages/FluentAssertions.Web.Serializers.NewtonsoftJson)


To set the default serializer to **Newtonsoft.Json** one, use the following configuration:

```csharp
FluentAssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();

```

The related `Newtonsoft.Json.JsonSerializerSetttings` used to configure the Newtonsoft.Json serializer is accesible via the `NewtonsoftJsonSerializerConfig.Options` static field. So if you want to add a custom converter, then the related setting is changed like this:

```csharp
NewtonsoftJsonSerializerConfig.Options.Converters.Add(new YesNoBooleanJsonConverter());
```

## Full API

|  *HttpResponseMessageAssertions* | Contains a number of methods to assert that an HttpResponseMessage is in the expected state related to the HTTP content. | 
| --- | --- |
| **Should().BeAs&lt;TModel&gt;()**  | Asserts that HTTP response content can be an equivalent representation of the expected model.  | 
| **Should().HaveHeader()**  | Asserts that an HTTP response has a named header.  | 
| **Should().NotHaveHeader()**  | Asserts that an HTTP response does not have a named header.  | 
| **Should().HaveHttpStatus()**  | Asserts that a HTTP response has a HTTP status with the specified code.  | 
| **Should().NotHaveHttpStatus()**  |  that a HTTP response does not have a HTTP status with the specified code.  | 
| **Should().MatchInContent()**  | Asserts that HTTP response has content that matches a wildcard pattern.  | 
| **Should().Satisfy&lt;TModel&gt;()**  |  Asserts that an HTTP response content can be a model that satisfies an assertion.  |
| **Should().Satisfy&lt;HttpResponseMessage&gt;()**  |  Asserts that an HTTP response content can be a model that satisfies an assertion.  |

|  *Should().HaveHeader().And.* | Contains a number of methods to assert that an HttpResponseMessage is in the expected state related to HTTP headers. | 
| --- | --- | 
| **BeEmpty()**  | Asserts that an existing HTTP header in a HTTP response has no values.  | 
| **NotBeEmpty()**  | Asserts that an existing HTTP header in a HTTP response has any values.  | 
| **BeValues()**  | Asserts that an existing HTTP header in a HTTP response has an expected list of header values.  | 
| **Match()**  | Asserts that an existing HTTP header in a HTTP response contains at least a value that matches a wildcard pattern.  | 

|  *Should().Be400BadRequest().And.* | Contains a number of methods to assert that an HttpResponseMessage is in the expected state related to HTTP Bad Request response | 
| --- | --- | 
| **HaveError()**  | Asserts that a Bad Request HTTP response content contains an error message identifiable by an expected field name and a wildcard error text.  | 
| **OnlyHaveError()**  | Asserts that a Bad Request HTTP response content contains only a single error message identifiable by an expected field name and a wildcard error text.  | 
| **NotHaveError()**  | Asserts that a Bad Request HTTP response content does not contain an error message identifiable by an expected field name and a wildcard error text.  | 
| **HaveErrorMessage()**  | Asserts that a Bad Request HTTP response content contains an error message identifiable by an wildcard error text.  | 

|  *Fine grained status assertions.* |  | 
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
| **Should().Be422UnprocessableEntity()**  | Asserts that a HTTP response has the HTTP status 422 Unprocessable Entity  | 
| **Should().Be426UpgradeRequired()**  | Asserts that a HTTP response has the HTTP status 426 UpgradeRequired  | 
| **Should().Be429TooManyRequests()**  | Asserts that a HTTP response has the HTTP status 422 Too Many Requests  | 
| **Should().Be500InternalServerError()**  | Asserts that a HTTP response has the HTTP status 500 Internal Server Error  | 
| **Should().Be501NotImplemented()**  | Asserts that a HTTP response has the HTTP status 501 Not Implemented  | 
| **Should().Be502BadGateway()**  | Asserts that a HTTP response has the HTTP status 502 Bad Gateway  | 
| **Should().Be503ServiceUnavailable()**  | Asserts that a HTTP response has the HTTP status 503 Service Unavailable  | 
| **Should().Be504GatewayTimeout()**  | Asserts that a HTTP response has the HTTP status 504 Gateway Timeout  | 
| **Should().Be505HttpVersionNotSupported()**  | Asserts that a HTTP response has the HTTP status 505 Http Version Not Supported  | 


### The HttpResponsesMessage assertions from FluentAssertions vs. FluentAssertions.Web

In the [6.4.0](https://fluentassertions.com/releases/#640) release the main library introduced a set of related assertions: *BeSuccessful, BeRedirection, HaveClientError, HaveServerError, HaveError, HaveStatusCode, NotHaveStatusCode*. 

This library can still be used with FluentAssertions and it did not become obsoleted, not only because of the rich set of assertions, but also for the comprehensive output messages that are displayed when the test fails, feature that is not present in the main library, but in FluentAssertions.Web one.

### FluentAssertions.Web vs FluentAssertions.Mvc vs FluentAssertions.Http


**FluentAssertions.Web** does not extend the ASP.NET Core Controllers assertions, if you are looking for that, then consider [FluentAssertions.Mvc](https://github.com/fluentassertions/fluentassertions.mvc).

When FluentAssertions.Web was created, [FluentAssertions.Http](https://github.com/balanikas/FluentAssertions.Http) also existed at the time, solving the same problem when considering the asserting language.
Besides the extra assertions added by FluentAssertions.Web, an important effort is put by this library on what happens when a test fails.