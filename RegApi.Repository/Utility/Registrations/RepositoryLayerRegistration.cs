using Microsoft.Extensions.DependencyInjection;

namespace RegApi.Repository.Utility.Registrations
{
    public static class RepositoryLayerRegistration
    {
        public static void AddRepositoryLayerRegistration(this IServiceCollection services)
        {
            services.AddRepositoryRegistration();
            services.AddUtilityRegistration();
        }
    }
}
