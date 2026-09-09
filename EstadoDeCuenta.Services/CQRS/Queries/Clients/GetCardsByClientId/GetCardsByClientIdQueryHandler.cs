using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Cards;


namespace EstadoDeCuenta.Services.CQRS.Queries.Clients.GetCardsByClientId
{
    public class GetCardsByClientIdQueryHandler : IQueryHandler<GetCardsByClientIdQuery, IEnumerable<CardResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCardsByClientIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CardResponseDto>> HandleAsync(GetCardsByClientIdQuery query)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(query.ClientId);

            if (client is null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el cliente con Id {query.ClientId}.");
            }

            var cards = await _unitOfWork.Cards.GetByClientIdAsync(query.ClientId);

            return _mapper.Map<IEnumerable<CardResponseDto>>(cards);
        }
    }
}

