using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EstadoDeCuenta.Infrastructure.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDBContext _context;

        public SettingsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<AccountStatementSetting?> GetAccountStatementSettingsAsync()
        {
            return await _context.AccountStatementSettings
                .OrderBy(s => s.Id)
                .FirstOrDefaultAsync();
        }
    }
}
