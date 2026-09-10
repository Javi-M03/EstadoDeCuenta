using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Queries.Movements.GetMovements
{
    public class GetMovementsQuery
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
