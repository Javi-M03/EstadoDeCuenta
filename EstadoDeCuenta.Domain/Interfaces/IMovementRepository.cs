using EstadoDeCuenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Domain.Interfaces
{
    public interface IMovementRepository
    {
        Task<Movement?> GetByIdAsync(int id);
        Task<IEnumerable<Movement>> GetByCardIdAsync(int cardId);
        Task<IEnumerable<Movement>> GetByCardIdAndDateAsync(int cardId, DateTime startDate, DateTime endDate);
        Task<Movement> AddAsync(Movement movement);
        Task<IEnumerable<Movement>> GetAllAsync();

    }
}
