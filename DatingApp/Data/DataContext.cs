using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }        
        public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<UserLike> Likes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // creating a primary key with the combination of both keys
            modelBuilder.Entity<UserLike>()
                .HasKey(k => new {k.SourceUserId, k.LikedUserId});

            // create many to many relation
            // source user like many user
            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            // itself liked by many user
            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}