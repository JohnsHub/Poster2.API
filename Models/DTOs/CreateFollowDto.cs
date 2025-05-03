using System.ComponentModel.DataAnnotations;

namespace Poster2.API.Models.DTOs
{
    public class CreateFollowDto
    {
        [Required]
        public Guid FolloweeId { get; set; }
    }
}
