using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.DTOs.Cards
{
    public class CardCreateRequestDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public decimal CardLimit { get; set; }
       
    }
}
