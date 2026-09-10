namespace EstadoDeCuenta.Services.CQRS.Commands.Settings.UpdateAccountStatementSettings
{
    public class UpdateAccountStatementSettingsCommand
    {
        public decimal InterestPercentage { get; set; }
        public decimal MinimumPaymentPercentage { get; set; }
    }
}
