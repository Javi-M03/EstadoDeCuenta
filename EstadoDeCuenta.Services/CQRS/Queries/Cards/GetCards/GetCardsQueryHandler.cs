using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Cards;


namespace EstadoDeCuenta.Services.CQRS.Queries.Cards.GetCards
{
    public class GetCardsQueryHandler : IQueryHandler<GetCardsQuery, IEnumerable<CardResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCardsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CardResponseDto>> HandleAsync(GetCardsQuery query)
        {
            var cards = await _unitOfWork.Cards.GetAllAsync();

            return _mapper.Map<IEnumerable<CardResponseDto>>(cards);
        }
    }
}
