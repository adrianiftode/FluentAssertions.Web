namespace Assertions.Web.Tests.Internal;

public class TryReadModelTests
{
    [Fact]
    public void Given_valid_json_Then_it_succeeds_with_the_deserialized_model()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "name": "Jane", "role": "admin" }""")
        };

        var (success, errorMessage, model) = response.TryReadModel(typeof(Dictionary<string, string>));

        success.Should().BeTrue();
        errorMessage.Should().BeNull();
        model.Should().BeOfType<Dictionary<string, string>>()
            .Which.Should().ContainKey("name").WhoseValue.Should().Be("Jane");
    }

    [Fact]
    public void Given_malformed_json_Then_it_fails_with_the_composed_error_message()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent("this is not json")
        };

        var (success, errorMessage, model) = response.TryReadModel(typeof(Dictionary<string, string>));

        success.Should().BeFalse();
        model.Should().BeNull();
        errorMessage.Should()
            .StartWith("Exception while deserializing the model with SystemTextJsonSerializer")
            .And.Contain(": ");
    }

    [Fact]
    public void Given_a_json_object_deserialized_into_a_tuple_Then_it_fails_with_the_tuple_guard_message()
    {
        using var response = new HttpResponseMessage
        {
            Content = new StringContent(/*lang=json,strict*/ """{ "name": "Jane" }""")
        };

        var (success, errorMessage, model) = response.TryReadModel(typeof((string Name, int Id)));

        success.Should().BeFalse();
        model.Should().BeNull();
        errorMessage.Should().Contain("A JSON object cannot be deserialized into")
            .And.Contain("tuple");
    }
}
