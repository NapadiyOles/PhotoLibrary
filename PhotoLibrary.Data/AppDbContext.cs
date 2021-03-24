using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Picture> Pictures { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Pictures)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}