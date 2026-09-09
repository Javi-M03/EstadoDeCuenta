using AutoMapper;
using EstadoDeCuenta.DTOs.Clients;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateClient;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateClientCommand, ClientResponseDto> _handler;

        public ClientsController(IMapper mapper, ICommandHandler<CreateClientCommand, ClientResponseDto> handler)
        {
            _mapper = mapper;
            _handler = handler;
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponseDto>> Create(ClientCreateRequestDto dto)
        {
            var command = _mapper.Map<CreateClientCommand>(dto);
            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }
    }
}
