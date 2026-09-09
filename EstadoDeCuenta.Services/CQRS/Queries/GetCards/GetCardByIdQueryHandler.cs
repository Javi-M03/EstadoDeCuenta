using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Queries.GetCards
{
    public class GetCardByIdQueryHandler : IQueryHandler<GetCardByIdQuery, CardResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCardByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CardResponseDto> HandleAsync(GetCardByIdQuery query)
        {
            var card = await _unitOfWork.Cards.GetByIdAsync(query.CardId);
            if(card == null)
            {
                throw new KeyNotFoundException($"No se encontró la tarjeta con Id {query.CardId}.");
            }

            return _mapper.Map<CardResponseDto>(card);
        }
    }
}
