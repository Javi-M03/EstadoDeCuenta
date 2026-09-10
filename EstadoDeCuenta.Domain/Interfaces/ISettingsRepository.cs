using EstadoDeCuenta.Domain.Entities;

namespace EstadoDeCuenta.Domain.Interfaces
{
    public interface ISettingsRepository
    {
        Task<AccountStatementSetting?> GetAccountStatementSettingsAsync();
    }
}
