using AutoMapper;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateMovement;
using EstadoDeCuenta.Services.CQRS.Queries.Movements.GetMovementById;
using EstadoDeCuenta.Services.CQRS.Queries.Movements.GetMovements;
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
        private readonly IQueryHandler<GetMovementByIdQuery, MovementResponseDto> _getByIdHandler;


        public MovementsController(IMapper mapper,
            ICommandHandler<CreateMovementCommand,MovementResponseDto> handler,
            IQueryHandler<GetMovementsQuery, IEnumerable<MovementResponseDto>> getHandler,
            IQueryHandler<GetMovementByIdQuery, MovementResponseDto> getByIdHandler
            )
        {
            _mapper = mapper;
            _handler = handler;
            _getHandler = getHandler;
            _getByIdHandler = getByIdHandler;
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
        public async Task<ActionResult<IEnumerable<MovementResponseDto>>> GetAll(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var query = new GetMovementsQuery
            {
                FromDate = fromDate,
                ToDate = toDate
            };
            var result = await _getHandler.HandleAsync(query);
            return Ok(result);
        }

        [HttpGet("{movementId}")]
        public async Task<ActionResult<MovementResponseDto>> GetById(int movementId)
        {
            var query = new GetMovementByIdQuery
            {
                MovementId = movementId
            };

            var result = await _getByIdHandler.HandleAsync(query);
            return Ok(result);
        }
    }
}
