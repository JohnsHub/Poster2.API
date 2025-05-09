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
        private readonly Poster2Context _context;

        // For testing - allows controlling what Guid is used in tests
        public static Guid? TestOverrideGuid { get; set; }

        public CommentController(Poster2Context context)
        {
            _context = context;
        }

        // GET: api/comments
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromQuery] Guid postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    UserId = c.User.Id,
                    UserName = c.User.UserName,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
            return Ok(comments);
        }

        // GET: api/comments/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetCommentById(Guid id)
        {
            var comment = await _context.Comments
                .Where(c => c.Id == id)
                .Include(c => c.User)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    PostId = c.PostId,
                    Content = c.Content,
                    UserId = c.User.Id,
                    UserName = c.User.UserName,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        // POST: api/comments
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!;

            // Create comment but don't add to context yet
            var comment = new Comment
            {
                Content = dto.Content,
                PostId = dto.PostId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            // If we're in a test, explicitly override the Id that was set by the Comment constructor
            if (TestOverrideGuid.HasValue)
            {
                // Use EF's property access method to override the Id even though it's been set
                _context.Entry(comment).Property(x => x.Id).CurrentValue = TestOverrideGuid.Value;
                TestOverrideGuid = null; // Reset for next time
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var result = new CommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                UserId = comment.UserId,
                UserName = User.Identity!.Name!,
                CreatedAt = comment.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetComments),
                new { postId = comment.PostId },
                result
            );
        }

        // PUT: api/comments/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(Guid id, [FromBody] UpdateCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!;

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            if (comment.UserId != userId) return Forbid();

            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new CommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                UserId = comment.UserId,
                UserName = User.Identity!.Name!,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };

            return Ok(result);
        }
    }
}