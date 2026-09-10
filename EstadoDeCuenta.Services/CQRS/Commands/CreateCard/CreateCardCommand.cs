using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateCard
{
    public class CreateCardCommand
    {
        public string CardNumber { get; set; } = string.Empty;
        public decimal CardLimit { get; set; }
        public int ClientId { get; set; }
    }
}
