namespace Poster2.API.Models.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CommentedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}
