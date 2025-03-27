using ClientRegistry.Domain.Models.Validations.Utils;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRegistry.Domain.Models.Validations
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
                RuleFor(c => c.Document.Length).Equal(CpfValidation.CpfLength)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres, mas foi fornecido {PropertyValue}.");
                RuleFor(c => CpfValidation.Validate(c.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });

            When(c => c.Type == "PJ", () =>
            {
                RuleFor(c => c.Document.Length).Equal(CnpjValidation.CnpjLength)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres, mas foi fornecido {PropertyValue}.");
                RuleFor(c => CnpjValidation.Validate(c.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}
