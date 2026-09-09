using AutoMapper;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateMovement;
using EstadoDeCuenta.Services.CQRS.Queries.GetMovements;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/movements")]
    public class MovementsController :ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateMovementCommand, MovementResponseDto> _handler;
        private readonly IQueryHandler<GetMovementsQuery, IEnumerable<MovementResponseDto>> _getHandler;

        public MovementsController(IMapper mapper,
            ICommandHandler<CreateMovementCommand,MovementResponseDto> handler,
            IQueryHandler<GetMovementsQuery, IEnumerable<MovementResponseDto>> getHandler)
        {
            _mapper = mapper;
            _handler = handler;
            _getHandler = getHandler;
        }

        [HttpPost("/api/cards/{cardId}/movements")]
        public async Task<ActionResult<MovementResponseDto>> Create(
           int cardId, CreateMovementRequestDto dto)
        {
            var command = _mapper.Map<CreateMovementCommand>(dto);
            command.CardId = cardId;
            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetAll()
        {
            var query = new GetMovementsQuery();
            var result = await _getHandler.HandleAsync(query);
            return Ok(result);
        }
    }
}
