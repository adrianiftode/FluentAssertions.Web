using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Sample.Api.Net22.Controllers;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Api.Net22.Tests
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
            response.Should().Be200Ok().And.BeAs(new[]
            {
                new { Author = "Adrian", Content = "Hey" },
                new { Author = "Johnny", Content = "Hey!" }
            });
        }

        [Fact]
        public async Task Get_Returns_Ok_With_CommentsList_With_TwoUniqueComments()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments");

            // Assert
            response.Should().Satisfy<IReadOnlyCollection<Comment>>(
                model =>
                {
                    model.Should().HaveCount(2);
                    model.Should().OnlyHaveUniqueItems(c => c.CommentId);
                }
            );
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_Ok_With_The_Expected_Comment()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
            response.Should().Be200Ok().And.BeAs(new
            {
                Author = "Adrian",
                Content = "Hey"
            });
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_A_NonSpam_Comment()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
            response.Should().Satisfy(givenModelStructure: new
            {
                Author = default(string),
                Content = default(string)
            }, assertion: model =>
            {
                model.Author.Should().NotBe("I DO SPAM!");
                model.Content.Should().NotContain("BUY MORE");
            });
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_Response_That_Satisfies_Several_Assertions()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
            response.Should().Satisfy(
                resp =>
                {
                    resp.Content.Headers.ContentRange.Should().BeNull();
                    resp.Content.Headers.Allow.Should().NotBeNull();
                }
            );
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
            response.Should().Be200Ok().And.BeAs(new
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
                .And.HaveErrorMessage("A non-empty request body is required.");
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

        [Fact]
        public async Task Post_WithNoAuthorButWithContent_ReturnsBadRequestWithAnErrorMessageRelatedToAuthorOnly()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(@"{
                                          ""content"": ""Hey, you...""
                                        }", Encoding.UTF8, "application/json"));

            // Assert
            response.Should().Be400BadRequest()
                .And.OnlyHaveError("Author", "The Author field is required.");
        }
    }
}
