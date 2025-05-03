namespace Poster2.API.Models.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public string? Website { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int PostsCount { get; set; } = 0;

        // / Navigation properties
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
