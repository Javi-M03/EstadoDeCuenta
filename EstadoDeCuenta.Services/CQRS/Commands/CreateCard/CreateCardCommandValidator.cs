using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateCard
{
    public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
    {
        public CreateCardCommandValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .WithMessage("El número de tarjeta es obligatorio.")
                .MaximumLength(19)
                .WithMessage("El número de tarjeta no puede superar los 19 caracteres.");

            RuleFor(x => x.CardLimit)
                .GreaterThan(0)
                .WithMessage("El límite de crédito debe ser mayor que 0.");

            RuleFor(x => x.ClientId)
                .GreaterThan(0)
                .WithMessage("El ClientId debe ser mayor que 0.");
        }
    }
}
