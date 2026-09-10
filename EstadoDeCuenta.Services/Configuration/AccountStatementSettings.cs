using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Configuration
{
    public class AccountStatementSettings
    {
        public decimal InterestPercentage { get; set; } = 25m;
        public decimal MinimunPaymentPercentage { get; set; } = 5m;
    }
}
