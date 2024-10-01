using Microsoft.Extensions.DependencyInjection;

namespace RegApi.Services.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering service layer components in the dependency injection container.
    /// </summary>
    public static class ServiceLayerRegistration
    {
        /// <summary>
        /// Registers service layer components, including AutoMapper profiles, service registrations, and validations.
        /// </summary>
        /// <param name="services">The service collection to which the components will be added.</param>
        public static void AddServiceLayerRegistration(this IServiceCollection services)
        {
            services.AddMapperRegistration();
            services.AddServiceRegistration();
            services.AddValidations();
        }
    }

}
