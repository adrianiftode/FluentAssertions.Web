using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sample.Api.Net22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Comment> Get() => new[]
        {
            new Comment { Author = "Adrian", Content = "Hey" }
        };

        [HttpGet("{id}")]
        public Comment Get(int id) => new Comment { Author = "Adrian", Content = "Hey", CommentId = id };

        [HttpPost]
        public Comment Post([FromBody] Comment value) => value;
    }

    public class Comment
    {
        [Required]
        public string Author { get; set; }
        [Required, StringLength(maximumLength: 500)]
        public string Content { get; set; }
        public int CommentId { get; set; }
    }
}