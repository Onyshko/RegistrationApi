using FluentValidation;
using RegApi.Services.Models;

namespace RegApi.Services.Validation.AccountValidation
{
    public class ForgotPasswordModelValidation: ExtendedAbstractValidation<ForgotPasswordModel>
    {
        public ForgotPasswordModelValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Cannot Be Empty")
                .EmailAddress().WithMessage("Email Must Be A Valid Email Address");

            RuleFor(x => x.ClientUri)
                .NotEmpty().WithMessage("ClientUri Cannot Be Empty")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("ClientUri Must Be A Valid Absolute URI");
        }
    }
}
