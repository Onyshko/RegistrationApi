using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RegApi.Services.Models;
using RegApi.Services.Validation.AccountValidation;
using RegApi.Services.Validation;

namespace RegApi.Services.Utility.Registrations
{
    public static class ValidationRegistration
    {
        public static void AddValidations(this IServiceCollection services)
        {
            services.AddScoped<IValidator<TicketModel>, TicketModelValidation>();
            services.AddScoped<IValidator<UserRegistrationModel>, UserRegistrationModelValidation>();
            services.AddScoped<IValidator<UserAuthenticationModel>, UserAuthenticationModelValidation>();
            services.AddScoped<IValidator<ResetPasswordModel>, ResetPasswordModelValidation>();
            services.AddScoped<IValidator<ForgotPasswordModel>, ForgotPasswordModelValidation>();
        }
    }
}
