using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Api.Tests
{
    public class CommentsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CommentsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_Returns_Ok_With_CommentsList()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments");

            // Assert
            response.Should().Be200Ok().And.HaveContent(new[]
            {
                new { Author = "Adrian", Content = "Hey" }
            });
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_Ok_With_The_Expected_Comment()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
            response.Should().Be200Ok().And.HaveContent(new
            {
                Author = "Adrian",
                Content = "Hey"
            });
        }

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

        [Fact]
        public async Task Post_ReturnsOkAndWithContent()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(@"{
                      ""author"": ""John"",
                      ""content"": ""Hey, you...""
                    }", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be200Ok().And.HaveContent(new
            {
                Author = "John",
                Content = "Hey, you..."
            });
        }

        [Fact]
        public async Task Post_WithNoContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent("", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("*The input does not contain any JSON tokens*");
        }

        [Fact]
        public async Task Post_WithNoAuthorAndNoContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(@"{
                        ""author"": """",
                        ""content"": """"
                    }", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.HaveError("Author", "The Author field is required.")
                .And.HaveError("Content", "The Content field is required.");
        }

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
                .And.HaveError("Author", "The Author field is required.")
                .And.NotHaveError("content");
        }
    }
}
