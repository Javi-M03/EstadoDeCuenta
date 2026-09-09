using AutoMapper;
using EstadoDeCuenta.DTOs.Movements;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateMovement;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/cards/{cardId}/movements")]
    public class MovementsController :ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateMovementCommand, MovementResponseDto> _handler;

        public MovementsController(IMapper mapper, ICommandHandler<CreateMovementCommand, MovementResponseDto >handler)
        {
            _mapper = mapper;
            _handler = handler;
        }

        [HttpPost]
        public async Task<ActionResult<MovementResponseDto>> Create(
           int cardId, CreateMovementRequestDto dto)
        {
            var command = _mapper.Map<CreateMovementCommand>(dto);
            command.CardId = cardId;
            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }
    }
}
