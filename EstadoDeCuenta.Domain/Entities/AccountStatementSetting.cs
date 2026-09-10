namespace EstadoDeCuenta.Domain.Entities
{
    public class AccountStatementSetting
    {
        public int Id { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal MinimumPaymentPercentage { get; set; }
    }
}
