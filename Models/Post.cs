using System.ComponentModel.DataAnnotations.Schema;

namespace Poster2.API.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        public int LikeCount { get; set; } = 0;
        public int RetweetCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to AppUser
        public AppUser User { get; set; } = new AppUser();
        public Guid UserId { get; set; }

        // Navigation properties
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Retweet> Retweets { get; set; } = new List<Retweet>();
        public List<Like> Likes { get; set; } = new List<Like>();
    }
}
