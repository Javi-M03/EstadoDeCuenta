using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateMovement
{
    public class CreateMovementCommandValidator : AbstractValidator<CreateMovementCommand>
    {
        public CreateMovementCommandValidator()
        {
            RuleFor(x => x.CardId).GreaterThan(0).WithMessage("El CardId debe ser mayor a 0.");
            RuleFor(x => x.MovementAmount).GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
            RuleFor(x => x.MovementDate).NotEmpty().WithMessage("La fecha del movimiento es obligatoria.");
            RuleFor(x => x.MovementDescription).MaximumLength(500);
            RuleFor(x => x.MovementType).IsInEnum().WithMessage("Tipo de movimiento no es valido.");
        }
    }
}
