namespace Poster2.API.Models.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int RetweetCount { get; set; }

        // Foreign key to AppUser
        public Guid UserId { get; set; }
        
    }

}
