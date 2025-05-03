using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Poster2.API.Data;
using Poster2.API.Models;
using Poster2.API.Models.DTOs;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Poster2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly Poster2Context _context;

        public PostsController(Poster2Context context)
        {
            _context = context;
        }

        // GET: api/posts
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {

            var posts = await _context.Posts
    .Include(p => p.User)
        .ThenInclude(u => u.Profile)
    .Include(p => p.Comments)
    .Include(p => p.Likes)
    .Include(p => p.Retweets)
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
    })
    .OrderByDescending(p => p.CreatedAt)
    .ToListAsync();


            return Ok(posts);
        }

        // GET: api/posts/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.User)
                .Select(post => new PostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    UserId = post.User.Id,
                    UserName = post.User.UserName,
                    DisplayName = post.User.DisplayName,
                    ProfilePicture = post.User.ProfilePicture,
                    LikeCount = post.Likes.Count,
                    CommentCount = post.Comments.Count,
                    RetweetCount = post.Retweets.Count,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (post == null) return NotFound();

            return Ok(post);
        }

        // POST: api/posts
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePost(PostDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return NotFound("User not found");
            var post = new Post
            {
                Content = dto.Content,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        // PUT: api/posts/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto dto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Convert string to Guid
            if (post.UserId != userId) return Forbid();

            post.Content = dto.Content;
            post.UpdatedAt = DateTime.UtcNow;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/posts/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task <ActionResult> DeletePost(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Convert string to Guid
            if (post.UserId != userId) return Forbid();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
