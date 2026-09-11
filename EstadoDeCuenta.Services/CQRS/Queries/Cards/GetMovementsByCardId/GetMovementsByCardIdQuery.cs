using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EstadoDeCuenta.Domain.Enums;

namespace EstadoDeCuenta.Services.CQRS.Queries.Cards.GetMovementsByCardId
{
    public class GetMovementsByCardIdQuery
    {
        public int CardId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MovementTypeEnum? MovementType { get; set; }
    }
}
