using FluentAssertions.Web.Internal;
using Newtonsoft.Json.Linq;
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
        #endregion

        #region GetStringValuesByKey
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
        public void GivenJson_WhenKeyDoesNotExist_ThenReturnsEmpty()
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

        [Fact]
        public void GivenJson_WhenKeyHasSingleValue_ThenReturnsCollection()
        {
            // Arrange
            var json = JObject.Parse(@"{
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
        public void GivenJsonWithAField_WhenGetChildrenKeysIsCalledWithThatField_ThenReturnsDirectKeys()
        {
            // Arrange
            var json = JObject.Parse(@"{
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
            var json = JObject.Parse(@"{
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
            var json = JObject.Parse(@"{
                    ""Author"": [
                        ""The Author field is required.""
                    ]
            }");

            // Act
            var result = json.GetChildrenKeys("");

            // Assert
            result.Should().BeEquivalentTo("Author");
        }
        #endregion

        #region GetParentKey
        [Fact]
        public void GivenJsonWithAFieldWithChildren_WhenGetParentKeyIsCalledWithOneOfTheChildren_ThenReturnsParent()
        {
            // Arrange
            var json = JObject.Parse(@"{ ""root"" : {
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
            var json = JObject.Parse(@"{ ""root"" : {
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
            var json = JObject.Parse(@"{ ""root"" : """" }");

            // Act
            var result = json.GetParentKey("root");

            // Assert
            result.Should().BeNull();
        }
        #endregion
    }
}