using Microsoft.Extensions.DependencyInjection;
using RegApi.Services.Mapping;

namespace RegApi.Services.Utility.Registrations
{
    public static class MapperRegistration
    {
        public static void AddMapperRegistration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(TicketProfile).Assembly);
        }
    }
}
