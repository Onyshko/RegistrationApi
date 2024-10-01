using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RegApi.Services.Models;
using RegApi.Services.Validation.AccountValidation;
using RegApi.Services.Validation;

namespace RegApi.Services.Utility.Registrations
{
    /// <summary>
    /// Provides extension methods for registering FluentValidation validators in the dependency injection container.
    /// </summary>
    public static class ValidationRegistration
    {
        /// <summary>
        /// Registers validators for various models in the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the validators will be added.</param>
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
