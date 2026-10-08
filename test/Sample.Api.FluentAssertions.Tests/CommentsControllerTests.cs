#if SH
using Shouldly;
using System.Linq;
#elif AAV
using AwesomeAssertions;
#else
using FluentAssertions;
#endif
using Microsoft.AspNetCore.Mvc.Testing;
using Sample.Api.Controllers;
using System.Collections.Generic;
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
#if SH
            response.ShouldBe200Ok();
            response.ShouldBeAs(new[]
            {
                new { Author = "Adrian", Content = "Hey" },
                new { Author = "Johnny", Content = "Hey!" }
            });
#else
            response.Should().Be200Ok().And.BeAs(new[]
            {
                new { Author = "Adrian", Content = "Hey" },
                new { Author = "Johnny", Content = "Hey!" }
            });
#endif
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_Ok_With_The_Expected_Comment()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
#if SH
            response.ShouldBe200Ok();
            response.ShouldBeAs(new
            {
                Author = "Adrian",
                Content = "Hey"
            });
#else
            response.Should().Be200Ok().And.BeAs(new
            {
                Author = "Adrian",
                Content = "Hey"
            });
#endif
        }

        [Fact]
        public async Task Get_Returns_Ok_With_CommentsList_With_TwoUniqueComments()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments");

            // Assert
#if SH
            response.ShouldSatisfy<IEnumerable<Comment>>(model =>
            {
                model.Count().ShouldBe(2);
                model.Select(c => c.CommentId).ShouldBeUnique();
            });
#else
            response.Should().Satisfy<IEnumerable<Comment>>(model => 
                    model.Should().HaveCount(2).And.OnlyHaveUniqueItems(c => c.CommentId));
#endif
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_A_NonSpam_Comment()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
#if SH
            response.ShouldSatisfy(new
            {
                Author = default(string),
                Content = default(string)
            }, model =>
            {
                model.Author.ShouldNotBe("I DO SPAM!");
                model.Content.ShouldNotContain("BUY MORE");
            });
#else
            response.Should().Satisfy(givenModelStructure: new
            {
                Author = default(string),
                Content = default(string)
            }, assertion: model =>
                {
                    model.Author.Should().NotBe("I DO SPAM!");
                    model.Content.Should().NotContain("BUY MORE");
                });
#endif
        }

        [Fact]
        public async Task Get_WithCommentId_Returns_Response_That_Satisfies_Several_Assertions()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
#if SH
            response.ShouldSatisfy(r =>
            {
                r.Content.Headers.ContentRange.ShouldBeNull();
                r.Content.Headers.Allow.ShouldNotBeNull();
            });
#else
            response.Should().Satisfy(
                    r =>
                    {
                        r.Content.Headers.ContentRange.Should().BeNull();
                        r.Content.Headers.Allow.Should().NotBeNull();
                    }
            );
#endif
        }

        [Fact]
        public async Task Get_Returns_Response_With_A_Certain_Header()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/comments/1");

            // Assert
#if SH
            response.ShouldHaveHeaderWithValue("x-vendor", "vendor", "we want to test the header has a specific value");
#else
            response.Should().HaveHeader("x-vendor").And.BeValue("vendor", "we want to test the header has a specific value");
#endif
        }

        [Fact]
        public async Task Post_ReturnsCreated()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(/*lang=json,strict*/ @"{
                      ""author"": ""John"",
                      ""content"": ""Hey, you...""
                    }", Encoding.UTF8, "application/json"));

            // Assert
#if SH
            response.ShouldBe201Created();
            response.ShouldHaveLocation();
            response.ShouldHaveLocationMatching("*/api/Comments/1");
#else
            response.Should().Be201Created().And.HaveLocation().And.Match("*/api/Comments/1");
#endif
        }

        [Fact]
        public async Task Post_ReturnsCreatedAndWithContent()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(/*lang=json,strict*/ @"{
                      ""author"": ""John"",
                      ""content"": ""Hey, you...""
                    }", Encoding.UTF8, "application/json"));

            // Assert
#if SH
            response.ShouldBe201Created();
            response.ShouldHaveLocation();
            response.ShouldHaveLocationWithValue("http://localhost/api/Comments/1");
            response.ShouldBeAs(new
            {
                Author = "John",
                Content = "Hey, you..."
            });
#else
            response.Should().Be201Created()
                .And.HaveLocation().And.BeValue("http://localhost/api/Comments/1")
                .And.BeAs(new
            {
                Author = "John",
                Content = "Hey, you..."
            });
#endif
        }

        [Fact]
        public async Task Post_WithNoContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent("", Encoding.UTF8, "application/json"));

            // Assert
#if NETCOREAPP2_1 || NETCOREAPP2_2 || NET5_0_OR_GREATER
#if SH
            response.ShouldBe400BadRequest();
            response.ShouldHaveErrorMessage("A non-empty request body is required.");
#else
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("A non-empty request body is required.");
#endif
#elif NETCOREAPP3_0 || NETCOREAPP3_1
#if SH
            response.ShouldBe400BadRequest();
            response.ShouldHaveErrorMessage("*The input does not contain any JSON tokens*");
#else
            response.Should().Be400BadRequest()
                .And.HaveErrorMessage("*The input does not contain any JSON tokens*");
#endif
#endif
        }

        [Fact]
        public async Task Post_WithNoAuthorAndNoContent_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(/*lang=json,strict*/ @"{
                        ""author"": """",
                        ""content"": """"
                    }", Encoding.UTF8, "application/json"));

            // Assert
#if SH
            response.ShouldBe400BadRequest();
            response.ShouldHaveError("Author", "The Author field is required.");
            response.ShouldHaveError("Content", "The Content field is required.");
#else
            response.Should().Be400BadRequest()
                .And.HaveError("Author", "The Author field is required.")
                .And.HaveError("Content", "The Content field is required.");
#endif
        }

        [Fact]
        public async Task Post_WithNoAuthor_ReturnsBadRequestWithUsefulMessage()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(/*lang=json,strict*/ @"{
                                          ""content"": ""Hey, you...""
                                        }", Encoding.UTF8, "application/json"));

            // Assert
#if SH
            response.ShouldBe400BadRequest();
            response.ShouldHaveError("Author", "The Author field is required.");
            response.ShouldNotHaveError("content");
#else
            response.Should().Be400BadRequest()
                .And.HaveError("Author", "The Author field is required.")
                .And.NotHaveError("content");
#endif
        }

        [Fact]
        public async Task Post_WithNoAuthorButWithContent_ReturnsBadRequestWithAnErrorMessageRelatedToAuthorOnly()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(/*lang=json,strict*/ @"{
                                          ""content"": ""Hey, you...""
                                        }", Encoding.UTF8, "application/json"));

            // Assert
#if SH
            response.ShouldBe400BadRequest();
            response.ShouldOnlyHaveError("Author", "The Author field is required.");
#else
            response.Should().Be400BadRequest()
                .And.OnlyHaveError("Author", "The Author field is required.");
#endif
        }

        [Fact]
        public async Task Post_WithNoAuthorButWithContent_Returns_Bad_Request_With_No_Location()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/comments", new StringContent(/*lang=json,strict*/ @"{
                                          ""content"": ""Hey, you...""
                                        }", Encoding.UTF8, "application/json"));

            // Assert
#if SH
            response.ShouldBe400BadRequest();
            response.ShouldNotHaveLocation("Bad Request responses are not designed to have Location headers.");
#else
            response.Should().Be400BadRequest()
                .And.NotHaveLocation("Bad Request responses are not designed to have Location headers.");
#endif
        }
    }
}
