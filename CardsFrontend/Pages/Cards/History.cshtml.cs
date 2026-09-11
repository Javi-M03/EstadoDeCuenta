using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Cards;

public class HistoryModel : PageModel
{
    private readonly IApiClient _apiClient;

    public HistoryModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public int CardId { get; set; }

    // Period: "month" (default) or "all".
    [BindProperty(SupportsGet = true)]
    public string Period { get; set; } = "month";

    // Type: "" (all), "Compra", or "Pago".
    [BindProperty(SupportsGet = true)]
    public string Type { get; set; } = string.Empty;

    public Card? Card { get; set; }
    public List<Movement> Movements { get; set; } = new();

    public List<SelectListItem> PeriodOptions { get; } = new()
    {
        new SelectListItem("Este mes", "month"),
        new SelectListItem("Todos los movimientos", "all"),
    };

    public List<SelectListItem> TypeOptions { get; } = new()
    {
        new SelectListItem("Todos los movimientos", ""),
        new SelectListItem("Compra", "Compra"),
        new SelectListItem("Pago", "Pago"),
    };

    public async Task OnGetAsync()
    {
        Card = await _apiClient.GetCardAsync(CardId);

        var (fromDate, toDate) = ResolveDateRange();
        var type = string.IsNullOrWhiteSpace(Type) ? null : Type;

        Movements = await _apiClient.GetCardMovementsAsync(CardId, fromDate, toDate, type);
    }

    // Translates the chosen period into the concrete [from, to) bounds the API expects.
    private (DateTime? From, DateTime? To) ResolveDateRange()
    {
        switch (Period)
        {
            case "all":
                return (null, null);

            case "month":
            default:
                var now = DateTime.Now;
                var start = new DateTime(now.Year, now.Month, 1);
                return (start, start.AddMonths(1));
        }
    }
}
