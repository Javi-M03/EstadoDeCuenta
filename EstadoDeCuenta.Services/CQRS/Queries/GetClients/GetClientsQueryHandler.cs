using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Clients;



namespace EstadoDeCuenta.Services.CQRS.Queries.GetClients
{
    public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, IEnumerable<ClientResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientResponseDto>> HandleAsync(GetClientsQuery query)
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();

            return _mapper.Map<IEnumerable<ClientResponseDto>>(clients);
        }
    }
}
