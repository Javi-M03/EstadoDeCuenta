using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Queries.GetClients
{
    public class GetClientByIdQueryHandler : IQueryHandler<GetClientByIdQuery, ClientResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientResponseDto> HandleAsync(GetClientByIdQuery query)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(query.ClientId);

            if (client == null)
            {
                throw new KeyNotFoundException($"No se encontró el cliente con Id {query.ClientId}.");
            }

            return _mapper.Map<ClientResponseDto>(client);
        }
    }
}