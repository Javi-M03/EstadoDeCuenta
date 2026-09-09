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
    public class ClientRepository : IClientRepository
    {
        private readonly AppDBContext _context;

        public ClientRepository(AppDBContext context) 
        {
            _context = context;
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == id);
        }

        public async Task<Client> AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            return client;
        }
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }
    }
}
