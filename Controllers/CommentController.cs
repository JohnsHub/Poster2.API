using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster2.API.Data;
using Poster2.API.Models;
using Poster2.API.Models.DTOs;
using System.Security.Claims;

namespace Poster2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly Poster2Context _context; // Database context
        public CommentController(Poster2Context context) // Constructor to inject the database context
        {
            _context = context; // Initialize the context
        }

        // GET: api/comments
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromQuery] Guid postId) // ← use [FromQuery] to get the postId from the query string
        {
            var comments = await _context.Comments // ← start with the comments
                .Where(c => c.PostId == postId) // ← filter by the postId
                .OrderBy(c => c.CreatedAt) // ← order by the creation date
                .Select(c => new CommentDto // ← select the properties we want
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    UserId = c.User.Id,
                    UserName = c.User.UserName,
                    CreatedAt = c.CreatedAt
                }) // ← map to the DTO
                .ToListAsync(); // ← execute the query
            return Ok(comments); // ← return the result
        }

        // GET: api/comments/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetCommentById(Guid id)
        {
            // 1) Filter by the incoming id
            var comment = await _context.Comments
                .Where(c => c.Id == id)               // ← ensure we only fetch the one we want
                .Include(c => c.User)                 // ← eager-load the author
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    UserId = c.User.Id,
                    UserName = c.User.UserName,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();               // ← returns null if not found

            // 2) Return 404 if it wasn’t in the database
            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        // POST: api/comments
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Identify the user first

            var comment = new Comment // ← create a new comment object
            {
                PostId = dto.PostId, // ← set the post ID
                Content = dto.Content, // 
                UserId = userId, // ← set the user ID
                CommentedAt = DateTime.UtcNow // ← set the timestamp
            };

            _context.Comments.Add(comment); // ← add the comment to the context
            await _context.SaveChangesAsync(); // setting the comment.Id

            var result = new CommentDto // ← create a new CommentDto to return
            {
                Id = comment.Id, // ← set the ID
                PostId = comment.PostId, // ← set the Post ID
                Content = comment.Content, // ← set the content
                CommentedAt = comment.CommentedAt, //  ← set the CommentedAt timestamp
                UserName = User.Identity!.Name! // ← set the User Name
            };

            return CreatedAtAction( // ← return the created comment
                nameof(GetComments), // ← specify the action name
                new { postId = comment.PostId }, //
                result
                    );
        }

        // DELETE: api/comments/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Identify the user first

            var comment = await _context.Comments.FindAsync(id); // Find the comment by ID
            if (comment == null) return NotFound(); // Comment not found

            if (comment.UserId != userId) // Authorization check
            {
                return Forbid(); // User is not authorized to delete this comment
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        // PUT: api/comments/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(Guid id, [FromBody] UpdateCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); // Validate the model

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Identify the user first

            var comment = await _context.Comments.FindAsync(id); // Find the comment by ID
            if (comment == null) return NotFound(); // Check if the comment exists

            if (comment.UserId != userId) return Forbid(); // Authorization check

            comment.Content = dto.Content; // Update the content
            comment.UpdatedAt = DateTime.UtcNow; // Update the timestamp

            await _context.SaveChangesAsync(); // Save changes to the database

            var result = new CommentDto // Create a new CommentDto to return
            {
                Id = comment.Id, // Set the ID
                PostId = comment.PostId, // Set the Post ID
                Content = comment.Content, // Set the content
                UserId = comment.UserId, // Set the User ID
                UserName = User.Identity!.Name!, // Set the User Name
                CreatedAt = comment.CreatedAt, // Set the CreatedAt timestamp
                UpdatedAt = comment.UpdatedAt // Set the UpdatedAt timestamp
            };

            return Ok(result); // Return the updated comment
        }
    }
}
