using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Cards;

public class CreateModel : PageModel
{
    private readonly IApiClient _apiClient;

    public CreateModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public int ClientId { get; set; }

    [BindProperty]
    public Card Card { get; set; } = new();

    public Client? Client { get; set; }

    public async Task OnGetAsync(int clientId)
    {
        ClientId = clientId;
        Client = await _apiClient.GetClientAsync(clientId);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Client = await _apiClient.GetClientAsync(ClientId);
            return Page();
        }

        try
        {
            Card.ClientId = ClientId;
            await _apiClient.CreateCardAsync(ClientId, Card);
            return RedirectToPage("/Clients/Details", new { clientId = ClientId });
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            Client = await _apiClient.GetClientAsync(ClientId);
            return Page();
        }
    }
}
