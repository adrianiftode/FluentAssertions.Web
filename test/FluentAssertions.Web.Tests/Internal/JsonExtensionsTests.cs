using FluentAssertions.Web.Internal;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal
{
    public class JsonExtensionsTests
    {
        [Fact]
        public void GivenJsonWithAKey_WhenHasKeyIsCalledWithThatKeyName_ThenReturnsTrue()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required.""
                    ]
                }
            }");

            // Act
            var result = json.HasKey("Author");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GivenJsonWithAKey_WhenHasKeyIsCalledWithADifferentKeyName_ThenReturnsFalse()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    ""Comment"": [
                        ""The Comment field is required.""
                    ]
                }
            }");

            // Act
            var result = json.HasKey("Author");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GivenJsonWithAnEmptyKey_WhenHasKeyIsCalledWithEmptyKey_ThenReturnsTrue()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    """": [
                        ""A non-empty request body is required.""
                    ]
                }
            }");

            // Act
            var result = json.HasKey("");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GivenJsonWithoutAnEmptyKey_WhenHasKeyIsCalledWithEmptyKey_ThenReturnsFalse()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    ""Comment"": [
                        ""The Comment field is required.""
                    ]
                }
            }");

            // Act
            var result = json.HasKey("");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GivenJsonWithAKey_WhenHasKeyIsCalledWithThatKeyNameButInDifferentCase_ThenReturnsTrue()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required.""
                    ]
                }
            }");

            // Act
            var result = json.HasKey("author");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GivenJson_WhenKeyExistsAndHasAnArrayOfStringValues_ThenReturnsTheStrings()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required.""
                    ]
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEquivalentTo(new[] { "The Author field is required." });
        }

        [Fact]
        public void GivenJson_WhenKeyDoesntExist_ThenReturnsEmpty()
        {
            // Arrange
            var json = JObject.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required.""
                    ]
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Comment");

            // Assert
            result.Should().BeEmpty();
        }
    }
}