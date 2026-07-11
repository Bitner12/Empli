using Domain.Entities;
using Infrastructures.Contexts.Configartion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructures.Contexts

{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Hour> Hours { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Contractor> Contractors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new WorkerConfiguration());
            builder.ApplyConfiguration(new HourConfiguration());
            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new ContractorConfiguration());
        }
    }
}
