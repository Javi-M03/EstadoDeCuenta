using AutoMapper;
using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.DTOs.Cards;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateCard;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/clients/{clientId}/cards")]
    public class CardsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateCardCommand, CardResponseDto> _handler;

        public CardsController(
            IMapper mapper,
            ICommandHandler<CreateCardCommand, CardResponseDto> handler)
        {
            _mapper = mapper;
            _handler = handler;
        }

        [HttpPost]
        public async Task<ActionResult<CardResponseDto>> Create(int clientId,
            CardCreateRequestDto dto)
        {
            var command = _mapper.Map<CreateCardCommand>(dto);
            command.ClientId = clientId;

            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }
    }
}
