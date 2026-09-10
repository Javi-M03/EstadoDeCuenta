using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Cards;

public class IndexModel : PageModel
{
    private readonly IApiClient _apiClient;

    public IndexModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<Card> Cards { get; set; } = new();
    public Dictionary<int, string> ClientNames { get; set; } = new();

    public async Task OnGetAsync()
    {
        Cards = await _apiClient.GetCardsAsync();
        var clients = await _apiClient.GetClientsAsync();
        ClientNames = clients.ToDictionary(c => c.ClientId, c => c.ClientName);
    }

    public string OwnerName(int clientId) =>
        ClientNames.TryGetValue(clientId, out var name) ? name : $"Client {clientId}";
}
