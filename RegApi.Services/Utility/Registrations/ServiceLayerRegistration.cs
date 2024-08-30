using Microsoft.Extensions.DependencyInjection;

namespace RegApi.Services.Utility.Registrations
{
    public static class ServiceLayerRegistration
    {
        public static void AddServiceLayerRegistration(this IServiceCollection services)
        {
            services.AddMapperRegistration();
            services.AddServiceRegistration();
            services.AddValidations();
        }
    }
}
