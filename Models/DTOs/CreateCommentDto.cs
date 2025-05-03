using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class CreateCommentDto
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
