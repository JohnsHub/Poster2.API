namespace Poster2.API.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        // Navigation properties
        public AppUser User { get; set; } = new AppUser();
        public Post Post { get; set; } = new Post();
        public DateTime CommentedAt { get; internal set; }
    }
}
