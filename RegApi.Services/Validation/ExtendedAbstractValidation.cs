using FluentValidation;

namespace RegApi.Services.Validation
{
    public class ExtendedAbstractValidation<T>: AbstractValidator<T>
    {
        protected bool BaAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

        protected bool ValidDateTime(DateTime date, DateTime from, DateTime to)
        {
            return date >= from && date <= to;
        }
    }
}
