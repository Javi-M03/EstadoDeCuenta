using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Movements;


namespace EstadoDeCuenta.Services.CQRS.Queries.Movements.GetMovements
{
    public class GetMovementsQueryHandler : IQueryHandler<GetMovementsQuery, IEnumerable<MovementResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMovementsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovementResponseDto>> HandleAsync(GetMovementsQuery query)
        {
            var movements = await _unitOfWork.Movements.GetAllFilteredAsync(query.FromDate, query.ToDate);

            return _mapper.Map<IEnumerable<MovementResponseDto>>(movements);
        }
    }
}