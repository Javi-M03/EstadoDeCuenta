using AutoMapper;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.DTOs.Settings;

namespace EstadoDeCuenta.Services.CQRS.Queries.Settings.GetAccountStatementSettings
{
    public class GetAccountStatementSettingsQueryHandler
        : IQueryHandler<GetAccountStatementSettingsQuery, AccountStatementSettingsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAccountStatementSettingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AccountStatementSettingsDto> HandleAsync(GetAccountStatementSettingsQuery query)
        {
            var settings = await _unitOfWork.Settings.GetAccountStatementSettingsAsync();

            if (settings is null)
            {
                throw new KeyNotFoundException("No se encontró la configuración del estado de cuenta.");
            }

            return _mapper.Map<AccountStatementSettingsDto>(settings);
        }
    }
}
