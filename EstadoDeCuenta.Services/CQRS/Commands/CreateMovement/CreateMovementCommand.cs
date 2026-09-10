using EstadoDeCuenta.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateMovement
{
    public class CreateMovementCommand
    {
        public int CardId { get; set; }
        public DateTime MovementDate { get; set; }
        public decimal MovementAmount { get; set; }
        public string? MovementDescription { get; set; }
        public MovementTypeEnum MovementType { get; set; }
    }
}
