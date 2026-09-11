using FluentValidation;

namespace EstadoDeCuenta.Services.CQRS.Commands.Settings.UpdateAccountStatementSettings
{
    public class UpdateAccountStatementSettingsCommandValidator
        : AbstractValidator<UpdateAccountStatementSettingsCommand>
    {
        public UpdateAccountStatementSettingsCommandValidator()
        {
            RuleFor(x => x.InterestPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("El porcentaje de interés debe estar entre 0 y 100.");

            RuleFor(x => x.MinimumPaymentPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("El porcentaje de saldo mínimo debe estar entre 0 y 100.");
        }
    }
}
