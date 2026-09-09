using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Movements;


namespace EstadoDeCuenta.Services.CQRS.Queries.Cards.GetMovementsByCardId
{
    public class GetMovementsByCardIdQueryHandler : IQueryHandler<GetMovementsByCardIdQuery, IEnumerable<MovementResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMovementsByCardIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MovementResponseDto>> HandleAsync(GetMovementsByCardIdQuery query)
        {
            var card = await _unitOfWork.Cards.GetByIdAsync(query.CardId);

            if (card is null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró la tarjeta con Id {query.CardId}.");
            }

            var movements = await _unitOfWork.Movements.GetByCardIdAsync(query.CardId);

            return _mapper.Map<IEnumerable<MovementResponseDto>>(movements);
        }
    }
}

