using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL
{
    public class ApplicationContext : IdentityDbContext<User, UserRole, int>
    {
        public DbSet<Product> Products { get; private set; }
        public DbSet<Sprint> Sprints { get; private set; }
        public DbSet<Feature> Features { get; private set; }
        public DbSet<User> Users { get; private set; }
        public DbSet<Task> Tasks { get; private set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>()
                .HasMany(t => t.Team)
                .WithMany(u => u.Tasks);

            modelBuilder.Entity<Sprint>()
                .HasMany(s => s.Features)
                .WithMany(f => f.Sprints);

            modelBuilder.Entity<Feature>()
                .HasOne(f => f.Metric)
                .WithMany(m => m.Features);

            modelBuilder.Entity<Feature>()
                .HasMany(f => f.Tasks)
                .WithMany(t => t.Features);
        }
    }
}