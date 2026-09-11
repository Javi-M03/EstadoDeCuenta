namespace CardsFrontend.Models;

public class Statement
{
    public string ClientName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public decimal CardLimit { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal CurrentMonthPurchases { get; set; }
    public decimal PreviousMonthPurchases { get; set; }
    public decimal BonusInterest { get; set; }
    public decimal MinimumPayment { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal TotalPaymentWithInterest { get; set; }
    public List<Movement> Movements { get; set; } = new();
}
