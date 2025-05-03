using Microsoft.AspNetCore.Identity;

namespace Poster2.API.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One-to-one nav property—no FK here, Profile holds UserId
        public Profile? Profile { get; set; }

        // Collections for related entities
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Retweet> Retweets { get; set; } = new List<Retweet>();
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Follow> Followers { get; set; } = new List<Follow>();
        public List<Follow> Following { get; set; } = new List<Follow>();
        public string Website { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
        public int FollowersCount { get; internal set; }
        public int FollowingCount { get; internal set; }
        public int PostsCount { get; internal set; }
    }
}
