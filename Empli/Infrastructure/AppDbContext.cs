using Empli.Domian;
using Empli.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Empli.Infrastructure
{
    public class AppDbContext : IdentityDbContext<User>
    {
         
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
    }

}
