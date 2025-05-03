using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class UpdateCommentDto
    {
        public Guid Id { get; set; }
        [Required, MaxLength (280)]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}
