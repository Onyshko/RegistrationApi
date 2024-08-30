using FluentValidation;
using RegApi.Services.Models;

namespace RegApi.Services.Validation.AccountValidation
{
    public class UserAuthenticationModelValidation: ExtendedAbstractValidation<UserAuthenticationModel>
    {
        public UserAuthenticationModelValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Cannot Be Empty")
                .EmailAddress().WithMessage("Email Must Be A Valid Email Address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Cannot Be Empty")
                .MinimumLength(7).WithMessage("Password Must Be At Least 7 Characters Long")
                .Matches("[A-Z]").WithMessage("Password Must Contain At Least One Uppercase Letter")
                .Matches("[a-z]").WithMessage("Password Must Contain At Least One Lowercase Letter")
                .Matches("[0-9]").WithMessage("Password Must Contain At Least One Digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password Must Contain At Least One Special Character");
        }
    }
}
