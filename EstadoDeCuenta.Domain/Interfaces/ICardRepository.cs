using EstadoDeCuenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Domain.Interfaces
{
    public interface ICardRepository
    {
        Task<Card?> GetByIdAsync(int id);
        Task<Card?> GetByIdWithClientAsync(int id);
        Task<IEnumerable<Card>> GetByClientIdAsync(int clientId);
        Task<Card> AddAsync(Card card);
        Task<IEnumerable<Card>> GetAllAsync();


    }
}
