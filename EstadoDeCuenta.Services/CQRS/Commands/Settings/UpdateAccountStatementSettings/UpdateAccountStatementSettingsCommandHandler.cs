using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Settings;
using FluentValidation;

namespace EstadoDeCuenta.Services.CQRS.Commands.Settings.UpdateAccountStatementSettings
{
    public class UpdateAccountStatementSettingsCommandHandler
        : ICommandHandler<UpdateAccountStatementSettingsCommand, AccountStatementSettingsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAccountStatementSettingsCommand> _validator;

        public UpdateAccountStatementSettingsCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UpdateAccountStatementSettingsCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<AccountStatementSettingsDto> HandleAsync(UpdateAccountStatementSettingsCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var settings = await _unitOfWork.Settings.GetAccountStatementSettingsAsync();

            if (settings is null)
            {
                throw new KeyNotFoundException("No se encontró la configuración del estado de cuenta.");
            }

            settings.InterestPercentage = command.InterestPercentage;
            settings.MinimumPaymentPercentage = command.MinimumPaymentPercentage;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountStatementSettingsDto>(settings);
        }
    }
}
