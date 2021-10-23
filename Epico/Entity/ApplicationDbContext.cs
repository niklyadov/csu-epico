using Microsoft.EntityFrameworkCore;

namespace Epico.Entity
{
    public sealed class ApplicationDbContext : DbContext
    {
        private DbSet<Test> TestEntites { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}