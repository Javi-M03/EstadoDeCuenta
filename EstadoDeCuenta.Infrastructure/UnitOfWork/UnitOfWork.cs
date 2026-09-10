using EstadoDeCuenta.Domain.Interfaces;
using EstadoDeCuenta.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public IClientRepository Clients { get; }
        public ICardRepository Cards { get; }
        public IMovementRepository Movements { get; }

        public UnitOfWork(
            AppDBContext context,
            IClientRepository clients,
            ICardRepository cards,
            IMovementRepository movements)
        {
            _context = context;
            Clients = clients;
            Cards = cards;
            Movements = movements;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
