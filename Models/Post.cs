using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poster2.API.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // foreign key
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        // real nav props
        public List<Comment> Comments { get; set; } = new();
        public List<Like> Likes { get; set; } = new();
        public List<Retweet> Retweets { get; set; } = new();

        // these are DTO‐only—EF should skip them:
        [NotMapped]
        public string DisplayName { get; set; }

        [NotMapped]
        public string ProfilePicture { get; set; }
    }
}
