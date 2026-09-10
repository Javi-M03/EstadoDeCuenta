using EstadoDeCuenta.DTOs.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Interfaces
{
    public interface ICardService
    {
        Task<CardResponseDto?> GetByIdAsync(int id); 
        Task<IEnumerable<CardResponseDto>> GetByClientIdAsync(int clientId);
    }
}
