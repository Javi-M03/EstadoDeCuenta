using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Clients;

public class DetailsModel : PageModel
{
    private readonly IApiClient _apiClient;

    public DetailsModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Client? Client { get; set; }
    public List<Card> Cards { get; set; } = new();

    public async Task OnGetAsync(int clientId)
    {
        Client = await _apiClient.GetClientAsync(clientId);
        Cards = await _apiClient.GetClientCardsAsync(clientId);
    }
}
