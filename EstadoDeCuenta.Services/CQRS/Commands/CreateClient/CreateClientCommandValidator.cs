using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        { 
            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio.")
                .MaximumLength(256)
                .WithMessage("El nombre del cliente no puede superar los 256 caracteres.");
        }

    }
}
