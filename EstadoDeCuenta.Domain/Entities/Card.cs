using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Domain.Entities
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public decimal CardLimit { get; set; }
        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;

        public ICollection<Movement> Movements { get; set; } = new List<Movement>();

    }
}
