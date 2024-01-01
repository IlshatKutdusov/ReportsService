using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Report> Report { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<File>()
                .HasIndex(e => e.Name)
                .IsUnique();

            builder.Entity<Report>()
                .HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
