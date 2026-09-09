using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Movements;


namespace EstadoDeCuenta.Services.CQRS.Queries.GetMovements
{
    public class GetMovementByIdQueryHandler : IQueryHandler<GetMovementByIdQuery, MovementResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMovementByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MovementResponseDto> HandleAsync(GetMovementByIdQuery query)
        {
            var movement = await _unitOfWork.Movements.GetByIdAsync(query.MovementId);

            if (movement == null)
            {
                throw new KeyNotFoundException($"No se encontró el movimiento con Id {query.MovementId}.");
            }

            return _mapper.Map<MovementResponseDto>(movement);
        }
    }
}
