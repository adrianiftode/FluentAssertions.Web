using FluentAssertions;
using FluentAssertions.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
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
            response.Should().Be200Ok(new[]
            {
                new Comment { Author = "Adrian", Content = "Hey" }
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
            response.Should().Be200Ok(new
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
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = @"{
              ""author"": ""John"",
              ""content"": ""Hey, you...""
            }";

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(content, Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be200Ok();
        }

        [Fact]
        public async Task Post_WithNoContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent("", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.WithError("", "A non-empty request body is required.");
        }

        [Fact]
        public async Task Post_WithNoAuthorAndNoContentContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = @"{
              ""author"": """",
              ""content"": """"
            }";

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(content, Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.WithError("Author", "The Author field is required.")
                .And.WithError("Content", "The Content field is required.");
        }

        [Fact]
        public async Task Post_WithNoAuthor_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = @"{
  ""content"": ""Hey, you...""
}";
            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(content, Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.WithError("Author", "The Author field is required.");
        }
    }

    public class Comment
    {
        public string Author { get; set; }
        public string Content { get; set; }
    }
}
