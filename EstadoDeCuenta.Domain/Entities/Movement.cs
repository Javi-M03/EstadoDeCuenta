using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstadoDeCuenta.Domain.Enums;

namespace EstadoDeCuenta.Domain.Entities
{
    public class Movement
    {
        public int MovementId { get; set; }
        public int CardId { get; set; }
        public DateTime MovementDate { get; set; }
        public decimal MovementAmount { get; set; }
        public string? MovementDescription { get; set; }
        public MovementTypeEnum MovementType { get; set; }

        public Card Card { get; set; } = null!;
    }
}
