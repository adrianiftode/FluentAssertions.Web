# Shouldly.Web

A [Shouldly](https://shouldly.io/) extension over the `HttpResponseMessage` object. It provides HTTP-specific assertions whose failure messages include the HTTP Request and Response, so test failures show the whole conversation and less time with debugging is spent.

> This is an unofficial, community-maintained extension and is **not endorsed by the Shouldly project**. It requires Shouldly >= 5.0.0-preview.2 and is itself a prerelease until Shouldly 5.0.0 is stable.

## Install

```shell
dotnet add package Shouldly.Web
```

## Example

With Shouldly the assertions are called directly on the response, and unlike the FluentAssertions/AwesomeAssertions flavours there is no `Should()`/`And` chain — each assertion is its own call:

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
    response.ShouldBe200Ok();
}
```

When the assertion fails, the test output follows the Shouldly layout (`should be`/`but was`) and continues with the complete HTTP response and the originating HTTP request.

## Documentation

The shared [Assertions.Web documentation](https://github.com/adrianiftode/FluentAssertions.Web/blob/master/docs/Assertions.Web/Readme.md) describes the assertions at a glance, the failure output and the configuration. The [Shouldly.Web section](https://github.com/adrianiftode/FluentAssertions.Web/blob/master/readme.md#shouldlyweb) of the repository readme maps every FluentAssertions assertion name to its Shouldly counterpart.