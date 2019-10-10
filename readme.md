
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
![FailedTest2](https://github.com/adrianiftode/FluentAssertions.Web/blob/master/docs/images/FailedTest2.png?raw=true)