using Microsoft.Extensions.DependencyInjection;
using RegApi.Repository.Implementations;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Utility.Registrations
{
    public static class UtilityRegistration
    {
        public static void AddUtilityRegistration(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
