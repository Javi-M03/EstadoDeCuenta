using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClientRepository Clients { get; }
        ICardRepository Cards { get; }
        IMovementRepository Movements { get; }
        ISettingsRepository Settings { get; }
        Task<int> SaveChangesAsync();
    }
}
