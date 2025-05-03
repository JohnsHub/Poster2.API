namespace Poster2.API.Models
{
    public class Retweet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AppUser User { get; set; } = new AppUser();
        public Post Post { get; set; } = new Post();
    }
}
