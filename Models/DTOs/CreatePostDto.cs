using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class CreatePostDto
    {
        [Required, MaxLength(280)]
        public string Content { get; set; }
    }
}
