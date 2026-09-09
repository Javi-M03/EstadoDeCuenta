using AutoMapper;
using EstadoDeCuenta.DTOs.Clients;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.CreateClient;
using EstadoDeCuenta.Services.CQRS.Queries.GetClients;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateClientCommand, ClientResponseDto> _handler;
        private readonly IQueryHandler<GetClientsQuery, IEnumerable<ClientResponseDto>> _getHandler;
        private readonly IQueryHandler<GetClientByIdQuery, ClientResponseDto> _getByIdHandler;

        public ClientsController(
            IMapper mapper,
            ICommandHandler<CreateClientCommand, ClientResponseDto> handler,
            IQueryHandler<GetClientsQuery, IEnumerable<ClientResponseDto>> getHandler,
            IQueryHandler<GetClientByIdQuery, ClientResponseDto> getByIdHandler
            )
        {
            _mapper = mapper;
            _handler = handler;
            _getHandler = getHandler;
            _getByIdHandler = getByIdHandler;
        }
        
        [HttpPost]
        public async Task<ActionResult<ClientResponseDto>> Create(ClientCreateRequestDto dto)
        {
            var command = _mapper.Map<CreateClientCommand>(dto);
            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetAll()
        {
            var query = new GetClientsQuery();
            var result = await _getHandler.HandleAsync(query);
            return Ok(result);
        }
        [HttpGet("{clientId}")]
        public async Task<ActionResult<ClientResponseDto>> GetById(
        int clientId)
        {
            var query = new GetClientByIdQuery
            {
                ClientId = clientId
            };

            var result = await _getByIdHandler.HandleAsync(query);
            return Ok(result);
        }
    }
}
