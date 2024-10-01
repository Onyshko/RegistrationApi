using Microsoft.Extensions.DependencyInjection;
using RegApi.Services.Implementations;
using RegApi.Services.Interfaces;

namespace RegApi.Services.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering service implementations in the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers service implementations for various services in the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ISasService, SasService>();
        }
    }

}
