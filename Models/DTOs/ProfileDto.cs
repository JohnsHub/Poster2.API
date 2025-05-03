namespace Poster2.API.Models.DTOs
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
