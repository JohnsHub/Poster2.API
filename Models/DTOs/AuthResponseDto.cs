namespace Poster2.API.Models.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? ProfilePicture { get; set; } 
    }
}
