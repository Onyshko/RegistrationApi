using Microsoft.Extensions.DependencyInjection;
using RegApi.Repository.Implementations;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering repository services in the dependency injection container.
    /// </summary>
    public static class RepositoryRegistration
    {
        /// <summary>
        /// Registers the repository services in the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the repository services will be added.</param>
        public static void AddRepositoryRegistration(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        }
    }

}
