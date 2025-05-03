using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster2.API.Data;
using Poster2.API.Models;
using Poster2.API.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Poster2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly Poster2Context _context;

        public ProfileController(Poster2Context context)
        {
            _context = context;
        }

        // GET: api/user
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetProfiles()
        {
            var profile = await _context.Users.ToListAsync();
            return Ok(profile);
        }

        // GET: api/user/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetProfileById(Guid id)
        {
            var profile = await _context.Users
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null) return NotFound();

            var dto = new UserProfileDto
            {
                Id = profile.Id,
                DisplayName = profile.DisplayName,
                UserName = profile.UserName,
                ProfilePicture = profile.ProfilePicture,
                Bio = profile.Bio,
                Location = profile.Location,
                Website = profile.Website,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
                FollowersCount = profile.Followers.Count,
                FollowingCount = profile.Following.Count,
                PostsCount = profile.Posts.Count,
                Posts = profile.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    UserId = p.User.Id,
                    UserName = p.User.UserName,
                    DisplayName = p.User.DisplayName,
                    ProfilePicture = p.User.ProfilePicture,
                    LikeCount = p.Likes.Count,
                    CommentCount = p.Comments.Count,
                    RetweetCount = p.Retweets.Count,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList()
            };

            return Ok(dto);
        }

        // PUT: api/user/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProfile(Guid id, UserProfileDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId != id) return Forbid();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Update the user properties
            user.DisplayName = dto.DisplayName;
            user.UserName = dto.UserName;
            user.ProfilePicture = dto.ProfilePicture;
            user.Bio = dto.Bio;
            user.Location = dto.Location;
            user.Website = dto.Website;
            user.UpdatedAt = DateTime.UtcNow;
            user.FollowersCount = dto.FollowersCount;
            user.FollowingCount = dto.FollowingCount;
            user.PostsCount = dto.PostsCount;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
