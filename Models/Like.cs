namespace Poster2.API.Models
{
    public class Like
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public AppUser User { get; set; } = new AppUser();
        public Post Post { get; set; } = new Post();
    }
}
