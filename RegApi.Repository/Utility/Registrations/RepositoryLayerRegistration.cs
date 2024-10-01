using Microsoft.Extensions.DependencyInjection;

namespace RegApi.Repository.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering the repository layer services in the dependency injection container.
    /// </summary>
    public static class RepositoryLayerRegistration
    {
        /// <summary>
        /// Registers repository and utility services in the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
        public static void AddRepositoryLayerRegistration(this IServiceCollection services)
        {
            services.AddRepositoryRegistration();
            services.AddUtilityRegistration();
        }
    }

}
