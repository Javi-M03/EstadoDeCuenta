using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Queries.Cards.GetMovementsByCardId
{
    public class GetMovementsByCardIdQuery
    {
        public int CardId { get; set; }
    }
}
