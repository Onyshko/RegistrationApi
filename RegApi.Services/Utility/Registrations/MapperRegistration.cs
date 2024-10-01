using Microsoft.Extensions.DependencyInjection;
using RegApi.Services.Mapping;

namespace RegApi.Services.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering AutoMapper profiles in the dependency injection container.
    /// </summary>
    public static class MapperRegistration
    {
        /// <summary>
        /// Registers AutoMapper profiles for mapping configurations.
        /// </summary>
        /// <param name="services">The service collection to which the AutoMapper profiles will be added.</param>
        public static void AddMapperRegistration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(TicketProfile).Assembly);
        }
    }

}
