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
    public class LikeController : ControllerBase
    {
        private readonly Poster2Context _context;

        public LikeController(Poster2Context context)
        {
            _context = context;
        }

        // GET: api/like
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikes() // Get all likes
        {
            var likes = await _context.Likes // Get all likes
                .Include(l => l.User) // Include the user who liked
                .Include(l => l.Post) // Include the post that was liked
                .Select(l => new LikeDto // Select the properties we want
                {
                    Id = l.Id, // Map the properties to the DTO
                    UserId = l.UserId, // Map the user ID
                    PostId = l.PostId, // Map the post ID
                    CreatedAt = l.CreatedAt, // Map the creation date
                }) // Map to the DTO
                .ToListAsync(); // Execute the query
            return Ok(likes); // Return the result
        }

        // POST: api/like
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LikeDto>> CreateLike([FromBody] Like like)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
          
            like.CreatedAt = DateTime.UtcNow; // Timestamp the like creation

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok(like);
        }

        // DELETE: api/like/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLike(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Get the user ID from the token
            
            var like = await _context.Likes.FindAsync(id); // Find the like by ID
            if (like == null) return NotFound(); // Check if the like exists

            if (like.UserId != userId) // Authorization check
                return Forbid(); // User is not authorized to delete this like

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
