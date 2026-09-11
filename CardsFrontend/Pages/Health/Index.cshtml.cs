using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Health;

public class IndexModel : PageModel
{
    private readonly IApiClient _apiClient;

    public IndexModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public ApiHealth Health { get; set; } = new();

    public async Task OnGetAsync()
    {
        Health = await _apiClient.GetHealthAsync();
    }
}
