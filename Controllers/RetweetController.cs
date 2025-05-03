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
    public class RetweetController : ControllerBase
    {
        private readonly Poster2Context _context; // Database context

        public RetweetController(Poster2Context context) // Constructor to inject the database context
        {
            _context = context; // Initialize the context
        }

        // GET: api/retweet
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetweetDto>>> GetRetweets()
        {
            var retweets = await _context.Retweets
                .Include (r => r.UserId) // Include the user who retweeted
                .Include(r => r.Post) // Include the post that was retweeted
                .Select (r => new RetweetDto
                {
                    Id = r.Id, // Map the properties to the DTO
                    UserId = r.UserId, // Map the user ID
                    PostId = r.PostId, // Map the post ID
                    CreatedAt = r.CreatedAt, // Map the creation date
                }).ToListAsync(); // Execute the query

            return Ok(retweets); // Return the result
        }

        // POST: api/retweet
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RetweetDto>> CreateRetweet([FromBody] CreateRetweetDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Get the user ID from the token

            // Create a new retweet object
            var retweet = new Retweet
            {
                PostId = dto.PostId, // Set the post ID
                UserId = userId, // Set the user ID
                CreatedAt = DateTime.UtcNow // Set the creation date
            };

            _context.Retweets.Add(retweet); // Add the retweet to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            var result = new RetweetDto
            {
                Id = retweet.Id, // Set the ID
                UserId = retweet.UserId, // Set the user ID
                PostId = retweet.PostId, // Set the post ID
                CreatedAt = retweet.CreatedAt // Set the creation date
            };

            return CreatedAtAction(nameof(GetRetweets), new { id = retweet.Id }, result); // Return the created retweet
        }

        // DELETE: api/retweet/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRetweet(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!; // Get the user ID from the token
            var retweet = await _context.Retweets.FindAsync(id); // Find the retweet by ID

            if (retweet == null) // Check if the retweet exists
                return NotFound(); // Return 404 if not found

            if (retweet.UserId != userId) // Check if the user is authorized to delete the retweet

                _context.Retweets.Remove(retweet); // Remove the retweet from the context
            await _context.SaveChangesAsync(); // Save changes to the database
            return NoContent(); // Return 204 No Content
        }


    }
}
