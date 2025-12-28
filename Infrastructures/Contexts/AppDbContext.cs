using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructures.Contexts

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Hour> Hours { get; set; }
    }
}
