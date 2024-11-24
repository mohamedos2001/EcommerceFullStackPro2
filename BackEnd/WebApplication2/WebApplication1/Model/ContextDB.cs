using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Model
{
    public class ContextDB : IdentityDbContext<AppLoacationUser>
    {
        public ContextDB()
        {

        }
        public ContextDB(DbContextOptions options)
        {
        }
        public DbSet<Product> Products { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=ContextDB;Trusted_Connection=True;Encrypt=False");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
