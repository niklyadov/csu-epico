using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Epico.Entity.DAL
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; private set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}