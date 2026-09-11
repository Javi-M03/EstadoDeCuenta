using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Movements;

public class DetailsModel : PageModel
{
    private readonly IApiClient _apiClient;

    public DetailsModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Movement? Movement { get; set; }

    public async Task OnGetAsync(int movementId)
    {
        Movement = await _apiClient.GetMovementAsync(movementId);
    }
}
