namespace Poster2.API.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string? DsiplayName { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
