namespace CardsFrontend.Models;

public class MovementCreateRequest
{
    public DateTime MovementDate { get; set; } = DateTime.Now;
    public decimal MovementAmount { get; set; }
    public string MovementDescription { get; set; } = string.Empty;
    public MovementTypeEnum MovementType { get; set; }
}
