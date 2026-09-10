using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Cards;

public class AddMovementModel : PageModel
{
    private readonly IApiClient _apiClient;

    public AddMovementModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public int CardId { get; set; }

    [BindProperty]
    public MovementCreateRequest Movement { get; set; } = new() { MovementDate = DateTime.Now };

    public List<SelectListItem> MovementTypeOptions { get; } =
        Enum.GetValues<MovementTypeEnum>()
            .Select(t => new SelectListItem(t.ToString(), ((int)t).ToString()))
            .ToList();

    public Card? Card { get; set; }

    public async Task OnGetAsync(int cardId)
    {
        CardId = cardId;
        Card = await _apiClient.GetCardAsync(cardId);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Card = await _apiClient.GetCardAsync(CardId);
            return Page();
        }

        try
        {
            await _apiClient.CreateMovementAsync(CardId, Movement);
            return RedirectToPage("Details", new { cardId = CardId });
        }
        catch (ApiException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            Card = await _apiClient.GetCardAsync(CardId);
            return Page();
        }
    }
}
