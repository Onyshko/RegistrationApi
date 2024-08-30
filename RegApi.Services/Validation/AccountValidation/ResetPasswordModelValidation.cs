using FluentValidation;
using RegApi.Services.Models;

namespace RegApi.Services.Validation.AccountValidation
{
    public class ResetPasswordModelValidation: ExtendedAbstractValidation<ResetPasswordModel>
    {
        public ResetPasswordModelValidation()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Cannot Be Empty")
                .MinimumLength(8).WithMessage("Password Must Be At Least 8 Characters Long")
                .Matches("[A-Z]").WithMessage("Password Must Contain At Least One Uppercase Letter")
                .Matches("[a-z]").WithMessage("Password Must Contain At Least One Lowercase Letter")
                .Matches("[0-9]").WithMessage("Password Must Contain At Least One Digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password Must Contain At Least One Special Character");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Cannot Be Empty")
                .EmailAddress().WithMessage("Email Must Be A Valid Email Address");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password Cannot Be Empty")
                .Equal(x => x.Password).WithMessage("Confirm Password Must Match Password");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token Cannot Be Empty");

        }
    }
}
