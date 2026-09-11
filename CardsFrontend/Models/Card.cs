namespace CardsFrontend.Models;

public class Card
{
    public int CardId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public decimal CardLimit { get; set; }
    public int ClientId { get; set; }
}
