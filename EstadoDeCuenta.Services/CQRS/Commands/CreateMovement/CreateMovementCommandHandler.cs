using AutoMapper;
using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Enums;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Infrastructure.UnitOfWork;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateMovement
{
    public class CreateMovementCommandHandler : ICommandHandler<CreateMovementCommand, MovementResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateMovementCommand> _validator;

        public CreateMovementCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateMovementCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<MovementResponseDto> HandleAsync(CreateMovementCommand command)
        {
            // Validamos el command
            var validationResult = await _validator.ValidateAsync(command);

            if(!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Verificamos que exista la tarjeta
            var card = await _unitOfWork.Cards.GetByIdAsync(command.CardId);

            if(card is null)
            {
                throw new KeyNotFoundException($"No se encontró la tarjeta con Id {command.CardId}");
            }

            // Obtenemos los movimientos de la tarjeta
            var movements = await _unitOfWork.Movements.GetByCardIdAsync(command.CardId);

            // Calculamos el saldo actual de la tarjeta
            var currentBalance = movements
                .Where(m => m.MovementType == MovementTypeEnum.Compra)
                .Sum(m => m.MovementAmount) - movements
                .Where(m => m.MovementType == MovementTypeEnum.Pago)
                .Sum(m => m.MovementAmount);

            // Calculamos saldo disponible (si es una compra)
            if(command.MovementType == MovementTypeEnum.Compra)
            {
                var availableBalance = card.CardLimit - currentBalance;

                if(command.MovementAmount > availableBalance)
                {
                    throw new InvalidOperationException("Esta compra supera el saldo disponible de la tarjeta");
                }
            }

            //Calculamos saldo a pagar (si es pago)
            if(command.MovementType == MovementTypeEnum.Pago)
            {

                if(command.MovementAmount > currentBalance)
                {
                    throw new InvalidOperationException("No se aceptan sobregiros");
                }
            }

            // Command -> Entity
            var movement = _mapper.Map<Movement>(command);

            //Guardamos movimiento
            await _unitOfWork.Movements.AddAsync(movement);

            // Confirmamos el cambio
            await _unitOfWork.SaveChangesAsync();

            // Entity -> DTO
            return _mapper.Map<MovementResponseDto>(movement);

        }
    }
}
