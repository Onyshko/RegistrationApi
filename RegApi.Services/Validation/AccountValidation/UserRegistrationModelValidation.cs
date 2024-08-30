using FluentValidation;
using RegApi.Services.Models;

namespace RegApi.Services.Validation.AccountValidation
{
    public class UserRegistrationModelValidation : ExtendedAbstractValidation<UserRegistrationModel>
    {
        public UserRegistrationModelValidation()
        {
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is Empty")
                .Length(2, 25).WithMessage("Length of {PropertyName} has to Have from 2 to 25 Letters")
                .Must(BaAValidName).WithMessage("{PropertyName} Contains Invalid Characters");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is Empty")
                .Length(2, 25).WithMessage("Length of {PropertyName} has to Have from 2 to 25 Letters")
                .Must(BaAValidName).WithMessage("{PropertyName} Contains Invalid Characters");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email не може бути порожнім")
                .EmailAddress().WithMessage("Невірний формат Email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Cannot Be Empty")
                .MinimumLength(7).WithMessage("Password Must Be At Least 7 Characters Long")
                .Matches("[A-Z]").WithMessage("Password Must Contain At Least One Uppercase Letter")
                .Matches("[a-z]").WithMessage("Password Must Contain At Least One Lowercase Letter")
                .Matches("[0-9]").WithMessage("Password Must Contain At Least One Digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password Must Contain At Least One Special Character");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password Cannot Be Empty")
                .Equal(x => x.Password).WithMessage("Confirm Password Must Match Password");

            RuleFor(x => x.ClientUri)
                .NotEmpty().WithMessage("ClientUri Cannot Be Empty")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("ClientUri Must Be A Valid Absolute URI");
        }
    }
}
