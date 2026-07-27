using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Comment> Get() => new[]
        {
            new Comment { Author = "Adrian", Content = "Hey", CommentId = 1},
            new Comment { Author = "Johnny", Content = "Hey!", CommentId = 2 }
        };

        [HttpGet("{id}", Name = "GetById")]
        public Comment GetById(int id)
        {
            Response.Headers["x-vendor"] = "vendor";
            return new Comment { Author = "Adrian", Content = "Hey", CommentId = id };
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public CreatedAtRouteResult Post([FromBody] Comment value)
        {
            value.CommentId = 1;
            return CreatedAtRoute("GetById",
                new
                {
                    id = value.CommentId
                },
                value);
        }
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