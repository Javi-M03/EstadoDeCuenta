using EstadoDeCuenta.DTOs.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Interfaces
{
    public interface IMovementService
    {
        Task<MovementResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<MovementResponseDto>> GetByCardIdAsync(int cardId, MovementFilterDto? filter);
        Task<MovementResponseDto> CreateAsync(int cardId, CreateMovementRequestDto dto);
    }
}
