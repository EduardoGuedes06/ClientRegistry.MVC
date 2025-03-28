using ClientRegistry.Domain.Models;
using ClientRegistry.MVC.Models.Validations.Utils;
using FluentValidation;

namespace ClientRegistry.MVC.Models.Validation
{
    public class ClientValidation : AbstractValidator<Client>
    {
        public ClientValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
                .Length(2, 100).WithMessage("O campo {PropertyName} deve ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório")
                .Matches(@"^\(\d{2}\) \d{4,5}-\d{4}$")
                .WithMessage("O campo {PropertyName} está em um formato inválido");

            When(c => c.Type == "PF", () =>
            {
                RuleFor(c => c.Document)
                    .Length(CpfValidation.CpfLength).WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres, mas foi fornecido {PropertyValue}.");

                RuleFor(c => c.Document)
                    .Must(CpfValidation.Validate)
                    .WithMessage("O documento fornecido é inválido.");
            });

            When(c => c.Type == "PJ", () =>
            {
                RuleFor(c => c.Document)
                    .Length(CnpjValidation.CnpjLength).WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres, mas foi fornecido {PropertyValue}.");

                RuleFor(c => c.Document)
                    .Must(CnpjValidation.Validate)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}
