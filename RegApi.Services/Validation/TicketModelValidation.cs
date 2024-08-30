using FluentValidation;
using RegApi.Services.Models;

namespace RegApi.Services.Validation
{
    public class TicketModelValidation: ExtendedAbstractValidation<TicketModel>
    {
        public TicketModelValidation()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is Empty")
                .Length(2, 25).WithMessage("Length of {PropertyName} has to Have from 2 to 25 Letters")
                .Must(BaAValidName).WithMessage("{PropertyName} Contains Invalid Characters");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Length(0, 150).WithMessage("Length of {PropertyName} has to Have Less Then 150 Symbols");

            RuleFor(x => x.Deadline)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeAVaildDeadline).WithMessage("{PropertyName} Contains Invalid Characters");
        }

        protected bool BeAVaildDeadline(DateTime date)
        {
            return ValidDateTime(date, DateTime.Now, DateTime.MaxValue);
        }
    }
}
