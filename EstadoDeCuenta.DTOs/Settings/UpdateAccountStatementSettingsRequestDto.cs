namespace EstadoDeCuenta.DTOs.Settings
{
    public class UpdateAccountStatementSettingsRequestDto
    {
        public decimal InterestPercentage { get; set; }
        public decimal MinimumPaymentPercentage { get; set; }
    }
}
