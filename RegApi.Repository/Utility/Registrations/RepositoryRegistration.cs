using Microsoft.Extensions.DependencyInjection;
using RegApi.Repository.Implementations;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Utility.Registrations
{
    public static class RepositoryRegistration
    {
        public static void AddRepositoryRegistration(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        }
    }
}
