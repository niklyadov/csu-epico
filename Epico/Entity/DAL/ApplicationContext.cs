using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL
{
    public class ApplicationContext : IdentityDbContext<User>
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
        }
    }
}