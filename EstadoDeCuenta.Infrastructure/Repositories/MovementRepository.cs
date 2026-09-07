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
    public class MovementRepository : IMovementRepository
    {
        private readonly AppDBContext _context;

        public MovementRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Movement?> GetByIdAsync(int id)
        {
            return await _context.Movements.FirstOrDefaultAsync(m => m.MovementId == id);
        }

        public async Task<IEnumerable<Movement>> GetByCardIdAsync(int cardId)
        {
            return await _context.Movements
                .Where(m => m.CardId == cardId)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movement>> GetByCardIdAndDateAsync(int cardId, DateTime startDate, DateTime endDate)
        {
            return await _context.Movements
                .Where(m => m.CardId == cardId &&
                        m.MovementDate >= startDate &&
                        m.MovementDate <= endDate)
                .OrderByDescending (m => m.MovementDate)
                .ToListAsync ();
        }

        public async Task<Movement> AddAsync(Movement movement)
        {
            await _context.Movements.AddAsync(movement);
            
            return movement;
        }
    }
}
