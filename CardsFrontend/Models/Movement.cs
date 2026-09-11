namespace CardsFrontend.Models;

public class Movement
{
    public int MovementId { get; set; }
    public DateTime MovementDate { get; set; }
    public decimal MovementAmount { get; set; }
    public string MovementDescription { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
}
