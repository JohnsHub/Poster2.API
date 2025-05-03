﻿using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength (6)]
        public string Password { get; set; }
        [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string DisplayName { get; set; }

    }
}
