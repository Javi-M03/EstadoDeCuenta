using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDBContext _context;

        public CardRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Card?> GetByIdAsync(int id)
        {
            return await _context.Cards.FirstOrDefaultAsync(c => c.CardId == id);
        }

        public async Task<Card?> GetByIdWithClientAsync(int id)
        {
            return await _context.Cards.Include(c => c.Client).FirstOrDefaultAsync(c => c.CardId == id);
        }

        public async Task<IEnumerable<Card>> GetByClientIdAsync(int clientId)
        {
            // Uses the stored procedure usp_GetCardsByClient (see Database scripts /
            // AddStoredProcedures migration).
            return await _context.Cards
                .FromSqlRaw("EXEC dbo.usp_GetCardsByClient @ClientId = {0}", clientId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Card> AddAsync(Card card)
        {
            await _context.Cards.AddAsync(card);

            return card;
        }

        public async Task<IEnumerable<Card>> GetAllAsync()
        {
            return await _context.Cards.ToListAsync();
        }
    }
}
