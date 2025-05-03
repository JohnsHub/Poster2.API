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
    public class FollowController : ControllerBase
    {
        private readonly Poster2Context _context; // Database context
        public FollowController(Poster2Context context) // Constructor to inject the database context
        {
            _context = context; // Initialize the context
        }
        // GET: api/follow
        [AllowAnonymous] // Allow anonymous access to this endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowDto>>> GetFollows()
        {
            var follows = await _context.Follows
                .Include(f => f.UserId) // Include the user who follows
                .Include(f => f.Followee) // Include the user who is followed
                .Select(f => new FollowDto
                {
                    Id = f.Id, // Map the properties to the DTO
                    FollowerId = f.FollowerId, // Map the follower ID
                    FolloweeId = f.FolloweeId, // Map the followed user ID
                    CreatedAt = f.CreatedAt, // Map the creation date
                }).ToListAsync(); // Execute the query
            
            return Ok(follows); // Return the result
        }

        // POST: api/follow
        [Authorize] // Require authorization for this endpoint
        [HttpPost]
        public async Task<ActionResult<FollowDto>> CreateFollow([FromBody] FollowDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Get the user ID from the token

            // Create a new follow object
            var follow = new Follow
            {
                FolloweeId = dto.FolloweeId, // Set the followed user ID
                FollowerId = dto.FollowerId, // Set the follower ID
                CreatedAt = DateTime.UtcNow // Set the creation date
            };

            _context.Follows.Add(follow); // Add the follow to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the created follow object
            var result = new FollowDto
            {
                Id = follow.Id, // Set the ID
                FollowerId = follow.FollowerId, // Set the follower ID
                FolloweeId = follow.FolloweeId, // Set the followed user ID
                CreatedAt = follow.CreatedAt // Set the creation date
            };
            // Return the created follow object
            return CreatedAtAction(nameof(GetFollows), new { id = follow.Id }, result);
        }

        // DELETE: api/follow/{id}
        [Authorize] // Require authorization for this endpoint
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFollow(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Get the user ID from the token

            var follow = await _context.Follows.FindAsync(id); // Find the follow by ID
            if (follow == null) return NotFound(); // Check if the follow exists

            if (follow.FollowerId != userId) // Check if the follow belongs to the user
                return Forbid(); // Return 403 Forbidden if not

            _context.Follows.Remove(follow); // Remove the follow from the context
            await _context.SaveChangesAsync(); // Save changes to the database

            return NoContent(); // Return 204 No Content
        }
    }
}
