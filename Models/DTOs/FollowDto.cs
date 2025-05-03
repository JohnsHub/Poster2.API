namespace Poster2.API.Models.DTOs
{
    public class FollowDto
    {
        public Guid Id { get; set; } // Unique identifier for the follow relationship
        public Guid FollwerId { get; set; }
        public string FollowerName { get; set; }
        public Guid FollowedId { get; set; }
        public string FolloweeName { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid FollowerId { get; internal set; }
        public Guid FolloweeId { get; internal set; }
    }
}
