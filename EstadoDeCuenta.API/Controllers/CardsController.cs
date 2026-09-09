using AutoMapper;
using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.DTOs.Cards;
using EstadoDeCuenta.DTOs.Clients;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateCard;
using EstadoDeCuenta.Services.CQRS.Queries.Cards.GetCardById;
using EstadoDeCuenta.Services.CQRS.Queries.Cards.GetCards;
using EstadoDeCuenta.Services.CQRS.Queries.Cards.GetMovementsByCardId;
using EstadoDeCuenta.Services.CQRS.Queries.Clients.GetCardsByClientId;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateCardCommand, CardResponseDto> _handler;
        private readonly IQueryHandler<GetCardsQuery, IEnumerable<CardResponseDto>> _getHandler;
        private readonly IQueryHandler<GetCardByIdQuery, CardResponseDto> _getByIdHandler;
        private readonly IQueryHandler<GetMovementsByCardIdQuery, IEnumerable<MovementResponseDto>> _getMovementHandler;



        public CardsController(
            IMapper mapper,
            ICommandHandler<CreateCardCommand, CardResponseDto> handler,
            IQueryHandler<GetCardsQuery, IEnumerable<CardResponseDto>> getHandler,
            IQueryHandler<GetCardByIdQuery, CardResponseDto> getByIdHandler,
            IQueryHandler<GetMovementsByCardIdQuery, IEnumerable<MovementResponseDto>> getMovementHandler)
        {
            _mapper = mapper;
            _handler = handler;
            _getHandler = getHandler;
            _getByIdHandler = getByIdHandler;
            _getMovementHandler = getMovementHandler;

        }

        [HttpPost("/api/clients/{clientId}/cards")]
        public async Task<ActionResult<CardResponseDto>> Create(int clientId,
            CardCreateRequestDto dto)
        {
            var command = _mapper.Map<CreateCardCommand>(dto);
            command.ClientId = clientId;

            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardResponseDto>>> GetAll()
        {
            var query = new GetCardsQuery();
            var result = await _getHandler.HandleAsync(query);
            return Ok(result);
        }

        [HttpGet("{cardId}")]
        public async Task<ActionResult<ClientResponseDto>> GetById(int cardId)
        {
            var query = new GetCardByIdQuery
            {
                CardId = cardId
            };

            var result = await _getByIdHandler.HandleAsync(query);
            return Ok(result);
        }

        [HttpGet("{cardId}/movements")]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetMovements(int cardId)
        {
            var query = new GetMovementsByCardIdQuery
            {
                CardId = cardId
            };

            var result = await _getMovementHandler.HandleAsync(query);

            return Ok(result);
        }
    }
}
