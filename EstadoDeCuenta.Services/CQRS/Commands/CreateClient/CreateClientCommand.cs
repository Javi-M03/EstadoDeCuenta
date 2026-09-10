using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.CQRS.Commands.CreateClient
{
    public class CreateClientCommand
    {
        public string ClientName { get; set; } = string.Empty;
    }
}
