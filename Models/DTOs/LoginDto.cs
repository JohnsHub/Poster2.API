using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
