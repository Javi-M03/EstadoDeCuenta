using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.DTOs.Movements
{
    public class MovementResponseDto
    {
        public int MovementId { get; set; }
        public DateTime MovementDate { get; set; }
        public decimal MovementAmount { get; set; }
        public string? MovementDescription { get; set; }
        public string MovementType { get; set; } = string.Empty;
    }
}
