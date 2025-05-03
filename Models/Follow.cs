namespace Poster2.API.Models
{
    public class Follow
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FollowerId { get; set; } // User who is following
        public Guid FolloweeId { get; set; } // User who is being followed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; } // User who created the follow relationship

        // Navigation properties
        public AppUser Follower { get; set; } = null!;
        public AppUser Followee { get; set; } = null!;
    }
}
