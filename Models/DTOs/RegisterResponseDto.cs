namespace Poster2.API.Models.DTOs
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}
