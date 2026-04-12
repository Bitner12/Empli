
using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Infrastructures.Contexts;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Infrastructures.Identity;
namespace Infrastructures
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructures (this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IHourRepository, HourRepository>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("psql"), b => b.MigrationsAssembly("Infrastructures"));
            });
            services.AddIdentityCore<User>()
               .AddEntityFrameworkStores<AppDbContext>();
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            services.AddScoped<ITokenService,TokenService>();
            
               
    

            return services;
            
        } 
    }
}
