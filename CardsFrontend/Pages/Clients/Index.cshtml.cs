using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Clients;

public class IndexModel : PageModel
{
    private readonly IApiClient _apiClient;

    public IndexModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<Client> Clients { get; set; } = new();

    public async Task OnGetAsync()
    {
        Clients = await _apiClient.GetClientsAsync();
    }
}
