using FluentAssertions.Web.Internal;
using System.Text.Json;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal
{
    public class JsonExtensionsTests
    {
        #region HasKey
        [Fact]
        public void GivenJsonWithAKey_WhenHasKeyIsCalledWithThatKeyName_ThenReturnsTrue()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
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
            using var json = JsonDocument.Parse(@"{
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
            using var json = JsonDocument.Parse(@"{
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
            using var json = JsonDocument.Parse(@"{
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
            using var json = JsonDocument.Parse(@"{
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
        #endregion

        #region GetStringValuesByKey
        [Fact]
        public void GivenJson_WhenKeyExistsAndHasAnArrayOfStringValues_ThenReturnsTheStrings()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required."",
                        ""The Author length exceeds 200 characters.""
                    ]
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEquivalentTo(new[] { "The Author field is required.", "The Author length exceeds 200 characters." });
        }

        [Fact]
        public void GivenJson_WhenKeyExistsAndHasAnArrayOfObjects_ThenReturnsAnEmptyCollection()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        {""description"" : ""The Author field is required.""},
                        {""description"" : ""The Author length exceeds 200 characters.""}
                    ]
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GivenJson_WhenKeyExistsAndHasAnArrayOfNull_ThenReturnsAnEmptyCollection()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        null,
                        null
                    ]
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GivenJson_WhenKeyDoesNotExist_ThenReturnsEmpty()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
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

        [Fact]
        public void GivenJson_WhenKeyHasSingleValue_ThenReturnsCollection()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": ""The Author field is required.""
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEquivalentTo(new[] { "The Author field is required." });
        }

        [Fact]
        public void GivenJson_WhenKeyHasAComplexObject_ThenReturnsEmpty()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": { ""Text"" : ""The Author field is required."" }
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GivenJson_WhenKeyHasNullValue_ThenReturnsEmpty()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": null
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GivenJson_WhenKeyHasTrueValue_ThenReturnsEmpty()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": true
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GivenJson_WhenKeyHasFalseValue_ThenReturnsEmpty()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": false
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEmpty();
        }
        #endregion

        #region GetChildrenKeys
        [Fact]
        public void GivenJsonWithAField_WhenGetChildrenKeysIsCalledWithThatField_ThenReturnsDirectKeys()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required.""
                    ],
                    ""Content"": [
                        ""The Content field is required.""
                    ]
                }
            }");

            // Act
            var result = json.GetChildrenKeys("errors");

            // Assert
            result.Should().BeEquivalentTo("Author", "Content");
        }

        [Fact]
        public void GivenJsonWithAnEmptyField_WhenGetChildrenKeysIsCalledByItsParent_ThenReturnsDirectKeys()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    """": [
                        ""The Author field is required.""
                    ]
                }
            }");

            // Act
            var result = json.GetChildrenKeys("errors");

            // Assert
            result.Should().BeEquivalentTo("");
        }

        [Fact]
        public void GivenJson_WhenGetChildrenKeysIsCalledWithNullOrEmpty_ThenReturnsDirectKeysOfRoot()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                    ""Author"": [
                        ""The Author field is required.""
                    ]
            }");

            // Act
            var result = json.GetChildrenKeys("");

            // Assert
            result.Should().BeEquivalentTo("Author");
        }

        [Fact]
        public void GivenJson_WhenArrayContainsANonStringValue_ThenItExcludeFromResult()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{
                ""errors"": {
                    ""Author"": [
                        ""The Author field is required."",
                        true,
                        false,
                        null,
                        []
                    ]
                }
            }");

            // Act
            var result = json.GetStringValuesByKey("Author");

            // Assert
            result.Should().BeEquivalentTo(new[] { "The Author field is required." });
        }
        #endregion

        #region GetParentKey
        [Fact]
        public void GivenJsonWithAFieldWithChildren_WhenGetParentKeyIsCalledWithOneOfTheChildren_ThenReturnsParent()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{ ""root"" : {
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ],
                        ""Content"": [
                            ""The Content field is required.""
                        ]
                    }
                }
            }");

            // Act
            var result = json.GetParentKey("content");

            // Assert
            result.Should().Be("errors");
        }

        [Fact]
        public void GivenJsonWithAFieldWithChildren_WhenGetParentKeyIsCalledWithNoneOfTheChildren_ThenReturnsNull()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{ ""root"" : {
                    ""errors"": {
                        ""Author"": [
                            ""The Author field is required.""
                        ],
                        ""Content"": [
                            ""The Content field is required.""
                        ]
                    }
                }
            }");

            // Act
            var result = json.GetParentKey("date");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GivenJsonAFieldAndChildren_WhenGetParentKeyIsCalledWithItself_ThenReturnsNull()
        {
            // Arrange
            using var json = JsonDocument.Parse(@"{ ""root"" : """" }");

            // Act
            var result = json.GetParentKey("root");

            // Assert
            result.Should().BeNull();
        }
        #endregion
    }
}