using EstadoDeCuenta.DTOs.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientResponseDto?> GetByIdAsync(int id);
    }
}
