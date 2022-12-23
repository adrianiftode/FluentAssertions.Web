using FluentAssertions.Web.Tests.TestModels;

namespace FluentAssertions.Web.Tests.Internal;

public class HttpContentExtensionsTests
{
    public static IEnumerable<object[]> TestSerializers() => new List<object[]>
            {
                new object[] { new SystemTextJsonSerializer() },
                new object[] { new NewtonsoftJsonSerializer() }
            };

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_As_Expected(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """
        {
          "errors": {
            "Author": [
              "The Author field is required."
            ]
          }
        }
        """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            errors = new Dictionary<string, string[]>()
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.errors.Should().ContainKey("Author");
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_Numbered_Enum_Value(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "type" : 2 } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            type = default(TestEnum)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.type.Should().Be(TestEnum.Type1);
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_Quoted_Numbered_Enum_Value(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "type" : "2" } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            type = default(TestEnum)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.type.Should().Be(TestEnum.Type1);
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_String_CamelCase_Enum_Value(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "type" : "Type1" } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            type = default(TestEnum)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.type.Should().Be(TestEnum.Type1);
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_String_PascalCase_Enum_Value(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "type" : "type2" } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            type = default(TestEnum)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.type.Should().Be(TestEnum.Type2);
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_Empty_String_Enum_Value(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "type" : "" } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            type = default(TestEnum?)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.type.Should().Be(default(TestEnum?));
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_Different_Property_Case(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "name" : "John" } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            Name = default(string)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("John");
    }

    [Theory]
    [MemberData(nameof(TestSerializers))]
    public async Task Reads_A_Json_With_Quoted_Integer_Value(ISerializer serializer)
    {
        // Arrange
        var content = new StringContent(/*lang=json,strict*/ """{ "number" : "100" } """);

        // Act
        var result = await content.ReadAsAsync(new
        {
            number = default(int)
        }, serializer);

        // Assert
        result.Should().NotBeNull();
        result!.number.Should().Be(100);
    }
}
