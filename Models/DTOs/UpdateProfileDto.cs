using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public string? Website { get; set; } = string.Empty;

    }
}
