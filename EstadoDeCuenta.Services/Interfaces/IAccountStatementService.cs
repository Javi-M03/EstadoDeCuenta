using EstadoDeCuenta.DTOs.Statemets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Interfaces
{
    public interface IAccountStatementService
    {
        Task<AccountStatementDto?> GetByCardIdAsync(int cardId);
    }
}
