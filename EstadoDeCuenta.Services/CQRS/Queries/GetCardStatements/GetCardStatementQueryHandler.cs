using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Statemets;
using EstadoDeCuenta.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Queries.GetCardStatements
{
    public class GetCardStatementQueryHandler : IQueryHandler<GetCardStatementQuery, AccountStatementDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AccountStatementCalculator _calculator;

        public GetCardStatementQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            AccountStatementCalculator calculator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _calculator = calculator;
        }
        public async Task<AccountStatementDto> HandleAsync(
        GetCardStatementQuery query)
        {
            var card = await _unitOfWork.Cards.GetByIdWithClientAsync(query.CardId);

            if (card is null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró la tarjeta con Id {query.CardId}.");
            }

            var movements = await _unitOfWork.Movements.GetByCardIdAsync(query.CardId);
            //valores quemados temporales
            const decimal interestPercentage = 25m;
            const decimal minimumPaymentPercentage = 5m;
            var statement = _calculator.Calculate(
                card,
                movements,
                interestPercentage,
                minimumPaymentPercentage);

            statement.Movements =
                _mapper.Map<IEnumerable<DTOs.Movements.MovementResponseDto>>(movements);

            return statement;
        }
    }
}
