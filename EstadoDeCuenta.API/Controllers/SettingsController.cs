using AutoMapper;
using EstadoDeCuenta.DTOs.Settings;
using EstadoDeCuenta.Services.CQRS;
using EstadoDeCuenta.Services.CQRS.Commands.Settings.UpdateAccountStatementSettings;
using EstadoDeCuenta.Services.CQRS.Queries.Settings.GetAccountStatementSettings;
using Microsoft.AspNetCore.Mvc;

namespace EstadoDeCuenta.API.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQueryHandler<GetAccountStatementSettingsQuery, AccountStatementSettingsDto> _getHandler;
        private readonly ICommandHandler<UpdateAccountStatementSettingsCommand, AccountStatementSettingsDto> _updateHandler;

        public SettingsController(
            IMapper mapper,
            IQueryHandler<GetAccountStatementSettingsQuery, AccountStatementSettingsDto> getHandler,
            ICommandHandler<UpdateAccountStatementSettingsCommand, AccountStatementSettingsDto> updateHandler)
        {
            _mapper = mapper;
            _getHandler = getHandler;
            _updateHandler = updateHandler;
        }

        [HttpGet("account-statement")]
        public async Task<ActionResult<AccountStatementSettingsDto>> GetAccountStatementSettings()
        {
            var result = await _getHandler.HandleAsync(new GetAccountStatementSettingsQuery());
            return Ok(result);
        }

        [HttpPut("account-statement")]
        public async Task<ActionResult<AccountStatementSettingsDto>> UpdateAccountStatementSettings(
            UpdateAccountStatementSettingsRequestDto dto)
        {
            var command = _mapper.Map<UpdateAccountStatementSettingsCommand>(dto);
            var result = await _updateHandler.HandleAsync(command);
            return Ok(result);
        }
    }
}
