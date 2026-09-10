using AutoMapper;
using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Cards;
using FluentValidation;


namespace EstadoDeCuenta.Services.CQRS.Commands.CreateCard;

    public class CreateCardCommandHandler : ICommandHandler<CreateCardCommand, CardResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCardCommand> _validator;

        public CreateCardCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateCardCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CardResponseDto> HandleAsync(
            CreateCardCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var client = await _unitOfWork.Clients
                .GetByIdAsync(command.ClientId);

            if (client is null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el cliente con Id {command.ClientId}.");
            }

            var card = _mapper.Map<Card>(command);

            await _unitOfWork.Cards.AddAsync(card);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CardResponseDto>(card);
        }
    }
