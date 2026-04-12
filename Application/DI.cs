using Application.Abstratctions;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Application
{
    public static class DI
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWorkerService,WorkerService>();
            services.AddScoped<IHourService,HourService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            
            return services;
        }
    }
}
