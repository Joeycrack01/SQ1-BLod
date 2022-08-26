using Microsoft.EntityFrameworkCore;
using SQ1Blog.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SQ1Blog.Helpers
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().ToTable("Article");

            modelBuilder.Entity<Article>()
                .HasOne<ApplicationUser>(sc => sc.User)
                .WithMany(s => s.Article)
                .HasForeignKey(sc => sc.CreatedBy);

            modelBuilder.Entity<Article>().ToTable("Article");

            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.RoleId });
            modelBuilder.Entity<IdentityRole<string>>().HasKey(p => new { p.Id });
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.UserId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId });
        }
    }

   
}