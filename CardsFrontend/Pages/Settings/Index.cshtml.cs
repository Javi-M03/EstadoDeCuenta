using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Settings;

public class IndexModel : PageModel
{
    private readonly IApiClient _apiClient;

    public IndexModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public AccountStatementSettings Settings { get; set; } = new();

    [TempData]
    public string? SuccessMessage { get; set; }

    public async Task OnGetAsync()
    {
        Settings = await _apiClient.GetAccountStatementSettingsAsync() ?? new();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await _apiClient.UpdateAccountStatementSettingsAsync(Settings);
            SuccessMessage = "Settings updated successfully.";
            return RedirectToPage();
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
