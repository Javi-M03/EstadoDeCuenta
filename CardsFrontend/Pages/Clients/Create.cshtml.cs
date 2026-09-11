using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Clients;

public class CreateModel : PageModel
{
    private readonly IApiClient _apiClient;

    public CreateModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public Client Client { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _apiClient.CreateClientAsync(Client);
            return RedirectToPage("Index");
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
