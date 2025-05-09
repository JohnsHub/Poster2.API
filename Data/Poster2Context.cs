using Microsoft.EntityFrameworkCore;
using Poster2.API.Models;

namespace Poster2.API.Data
{
    public class Poster2Context : DbContext
    {
        public Poster2Context(DbContextOptions<Poster2Context> options) : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Retweet> Retweets { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Like entity relationships
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configure the Retweet entity relationships
            modelBuilder.Entity<Retweet>()
                .HasOne(r => r.User)
                .WithMany(u => u.Retweets)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Retweet>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Retweets)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configure the Comment entity relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Configure the Follow entity relationships
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the Profile entity relationships
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Profile when User is deleted

            modelBuilder.Entity<AppUser>()
    .Property(u => u.Website)
    .HasDefaultValue(string.Empty);

        }
    }
}
