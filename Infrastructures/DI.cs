
using Domain.Abstractions.Interfaces.Repositories;
using Infrastructures.Contexts;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructures (this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ManagerDb"));
            });
            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IHourRepository, HourRepository>();
            return services;
        } 
    }
}
