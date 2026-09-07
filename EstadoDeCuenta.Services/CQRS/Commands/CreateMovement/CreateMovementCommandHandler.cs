using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Infrastructure.UnitOfWork;
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

        public CreateMovementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MovementResponseDto> HandleAsync(CreateMovementCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
