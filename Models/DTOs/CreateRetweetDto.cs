namespace Poster2.API.Models.DTOs
{
    public class CreateRetweetDto
    {
        public Guid PostId { get; set; } // The ID of the post being retweeted
        public Guid UserId { get; set; } // The ID of the user who retweeted
        public DateTime CreatedAt { get; set; } // The date and time when the retweet was created
    }
}
