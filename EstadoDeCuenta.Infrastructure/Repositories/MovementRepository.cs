using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Enums;
using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.Infrastructure.Data;
using Microsoft.Data.SqlClient;
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
            // Uses the stored procedure usp_GetMovementsByCard (see Database scripts /
            // AddStoredProcedures migration). Results are ordered inside the procedure;
            // a stored-procedure call cannot be composed with further LINQ operators.
            return await _context.Movements
                .FromSqlRaw("EXEC dbo.usp_GetMovementsByCard @CardId = {0}", cardId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Movement>> GetByCardIdFilteredAsync(
            int cardId,
            DateTime? fromDate,
            DateTime? toDate,
            MovementTypeEnum? movementType)
        {
            // Uses the stored procedure usp_GetMovementsByCardFiltered. Each argument
            // is optional; a NULL parameter disables that part of the filter.
            var parameters = new[]
            {
                new SqlParameter("@CardId", cardId),
                new SqlParameter("@FromDate", (object?)fromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object?)toDate ?? DBNull.Value),
                new SqlParameter("@MovementType",
                    movementType.HasValue ? (object)(int)movementType.Value : DBNull.Value),
            };

            return await _context.Movements
                .FromSqlRaw(
                    "EXEC dbo.usp_GetMovementsByCardFiltered " +
                    "@CardId = @CardId, @FromDate = @FromDate, @ToDate = @ToDate, @MovementType = @MovementType",
                    parameters)
                .AsNoTracking()
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
        public async Task<IEnumerable<Movement>> GetAllAsync()
        {
            return await _context.Movements.ToListAsync();
        }

        public async Task<IEnumerable<Movement>> GetAllFilteredAsync(DateTime? fromDate, DateTime? toDate)
        {
            // Uses the stored procedure usp_GetMovementsFiltered (optional date range).
            var parameters = new[]
            {
                new SqlParameter("@FromDate", (object?)fromDate ?? DBNull.Value),
                new SqlParameter("@ToDate", (object?)toDate ?? DBNull.Value),
            };

            return await _context.Movements
                .FromSqlRaw(
                    "EXEC dbo.usp_GetMovementsFiltered @FromDate = @FromDate, @ToDate = @ToDate",
                    parameters)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
