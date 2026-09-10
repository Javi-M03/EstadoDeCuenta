using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Queries.Clients.GetCardsByClientId
{
    public class GetCardsByClientIdQuery
    {
        public int ClientId { get; set; }
    }
}
