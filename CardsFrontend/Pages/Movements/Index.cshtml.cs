using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Movements;

public class IndexModel : PageModel
{
    private readonly IApiClient _apiClient;

    public IndexModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Period: "month" (default), "last", or "all".
    [BindProperty(SupportsGet = true)]
    public string Period { get; set; } = "month";

    public List<Movement> Movements { get; set; } = new();

    public List<SelectListItem> PeriodOptions { get; } = new()
    {
        new SelectListItem("Mes Actual", "month"),
        new SelectListItem("Mes Anterior", "last"),
        new SelectListItem("Todos los movimientos", "all"),
    };

    public async Task OnGetAsync()
    {
        var (fromDate, toDate) = ResolveDateRange();
        Movements = await _apiClient.GetMovementsAsync(fromDate, toDate);
    }

    private (DateTime? From, DateTime? To) ResolveDateRange()
    {
        var now = DateTime.Now;
        var thisMonthStart = new DateTime(now.Year, now.Month, 1);

        switch (Period)
        {
            case "all":
                return (null, null);

            case "last":
                return (thisMonthStart.AddMonths(-1), thisMonthStart);

            case "month":
            default:
                return (thisMonthStart, thisMonthStart.AddMonths(1));
        }
    }
}
