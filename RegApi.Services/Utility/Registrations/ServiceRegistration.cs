using Microsoft.Extensions.DependencyInjection;
using RegApi.Services.Implementations;
using RegApi.Services.Interfaces;

namespace RegApi.Services.Utility.Registrations
{
    public static class ServiceRegistration
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ITicketService, TicketService>();
        }
    }
}
