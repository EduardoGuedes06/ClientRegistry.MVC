using ClientRegistry.MVC.Models.Data;
using FluentValidation;

namespace ClientRegistry.MVC.Models.Validation
{
    public class DateValidation : AbstractValidator<DataViewModel>
    {
        public DateValidation()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("A data de início é obrigatória.")
                .Must(BeAValidDate).WithMessage("A data de início não é válida.")
                .Must(BeWithinLastFiveYears).WithMessage("A data de início não pode ultrapassar 5 anos.")
                .Must((model, startDate) => !string.IsNullOrEmpty(model.EndDate) && DateTime.TryParse(model.EndDate, out DateTime endDate) && DateTime.TryParse(startDate, out DateTime start) && start <= endDate)
                .WithMessage("A data de início não pode ser maior que a data de término.")
                .Must(BeNotInTheFuture).WithMessage("A data de início não pode ser no futuro.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("A data de término é obrigatória.")
                .Must(BeAValidDate).WithMessage("A data de término não é válida.")
                .Must(BeWithinLastFiveYears).WithMessage("A data de término não pode ultrapassar 5 anos.")
                .Must((model, endDate) => !string.IsNullOrEmpty(model.StartDate) && DateTime.TryParse(model.StartDate, out DateTime startDate) && DateTime.TryParse(endDate, out DateTime end) && end >= startDate)
                .WithMessage("A data de término não pode ser anterior à data de início.")
                .Must(BeNotInTheFuture).WithMessage("A data de término não pode ser no futuro.");
        }

        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }

        private bool BeWithinLastFiveYears(string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                return parsedDate >= DateTime.Now.AddYears(-5);
            }
            return false;
        }

        private bool BeNotInTheFuture(string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                var endOfYear = new DateTime(DateTime.Now.Year, 12, 31);
                return parsedDate <= endOfYear;
            }
            return false;
        }
    }

}
