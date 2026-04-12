using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructures.Contexts

{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Hour> Hours { get; set; }
    }
}
