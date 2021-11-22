using FluentAssertions.Web.Internal;
using FluentAssertions.Web.Tests.TestModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal
{
    public class HttpContentExtensionsTests
    {
        [Fact]
        public async Task Reads_A_Json_As_Expected()
        {
            // Arrange
            var content = new StringContent(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required.""
                    ]
                }
            }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                errors = new Dictionary<string, string[]>()
            });

            // Assert
            result.Should().NotBeNull();
            result!.errors.Should().ContainKey("Author");
        }

        [Fact]
        public async Task Reads_A_Json_With_Numbered_Enum_Value()
        {
            // Arrange
            var content = new StringContent(@"{ ""type"" : 2 }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                type = default(TestEnum)
            });

            // Assert
            result.Should().NotBeNull();
            result!.type.Should().Be(TestEnum.Type1);
        }

        [Fact]
        public async Task Reads_A_Json_With_Quoted_Numbered_Enum_Value()
        {
            // Arrange
            var content = new StringContent(@"{ ""type"" : ""2"" }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                type = default(TestEnum)
            });

            // Assert
            result.Should().NotBeNull();
            result!.type.Should().Be(TestEnum.Type1);
        }

        [Fact]
        public async Task Reads_A_Json_With_String_CamelCase_Enum_Value()
        {
            // Arrange
            var content = new StringContent(@"{ ""type"" : ""Type1"" }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                type = default(TestEnum)
            });

            // Assert
            result.Should().NotBeNull();
            result!.type.Should().Be(TestEnum.Type1);
        }

        [Fact]
        public async Task Reads_A_Json_With_String_PascalCase_Enum_Value()
        {
            // Arrange
            var content = new StringContent(@"{ ""type"" : ""type2"" }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                type = default(TestEnum)
            });

            // Assert
            result.Should().NotBeNull();
            result!.type.Should().Be(TestEnum.Type2);
        }

        [Fact]
        public async Task Reads_A_Json_With_Empty_String_Enum_Value()
        {
            // Arrange
            var content = new StringContent(@"{ ""type"" : """" }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                type = default(TestEnum?)
            });

            // Assert
            result.Should().NotBeNull();
            result!.type.Should().Be(default(TestEnum?));
        }

        [Fact]
        public async Task Reads_A_Json_With_Different_Property_Case()
        {
            // Arrange
            var content = new StringContent(@"{ ""name"" : ""John"" }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                Name = default(string)
            });

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("John");
        }

        [Fact]
        public async Task Reads_A_Json_With_Quoted_Integer_Value()
        {
            // Arrange
            var content = new StringContent(@"{ ""number"" : ""100"" }");

            // Act
            var result = await content.ReadAsAsync(new
            {
                number = default(int)
            });

            // Assert
            result.Should().NotBeNull();
            result!.number.Should().Be(100);
        }
    }
}
