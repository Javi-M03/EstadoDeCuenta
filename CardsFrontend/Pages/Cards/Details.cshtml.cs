using CardsFrontend.Models;
using CardsFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CardsFrontend.Pages.Cards;

public class DetailsModel : PageModel
{
    private const string ExcelContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private readonly IApiClient _apiClient;
    private readonly IStatementExportService _exportService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(
        IApiClient apiClient,
        IStatementExportService exportService,
        ILogger<DetailsModel> logger)
    {
        _apiClient = apiClient;
        _exportService = exportService;
        _logger = logger;
    }

    public Card? Card { get; set; }
    public Client? Client { get; set; }
    public List<Movement> Movements { get; set; } = new();
    public Statement? Statement { get; set; }

    public async Task OnGetAsync(int cardId)
    {
        Card = await _apiClient.GetCardAsync(cardId);
        Movements = await _apiClient.GetCardMovementsAsync(cardId);
        Statement = await _apiClient.GetCardStatementAsync(cardId);

        if (Card is not null)
        {
            Client = await _apiClient.GetClientAsync(Card.ClientId);
        }
    }

    public async Task<IActionResult> OnGetExportPdfAsync(int cardId)
    {
        var statement = await _apiClient.GetCardStatementAsync(cardId);
        if (statement is null)
        {
            return NotFound();
        }

        try
        {
            var bytes = _exportService.BuildPdf(statement);
            return File(bytes, "application/pdf", BuildFileName(statement, "pdf"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate PDF statement for card {CardId}", cardId);
            throw;
        }
    }

    public async Task<IActionResult> OnGetExportExcelAsync(int cardId)
    {
        var statement = await _apiClient.GetCardStatementAsync(cardId);
        if (statement is null)
        {
            return NotFound();
        }

        try
        {
            var bytes = _exportService.BuildExcel(statement);
            return File(bytes, ExcelContentType, BuildFileName(statement, "xlsx"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate Excel statement for card {CardId}", cardId);
            throw;
        }
    }

    private static string BuildFileName(Statement statement, string extension)
    {
        var client = Slug(statement.ClientName, "client");
        var card = Slug(statement.CardNumber, "card");
        return $"statement_{client}_{card}_{DateTime.Now:yyyyMMdd}.{extension}";
    }

    private static string Slug(string? value, string fallback)
    {
        var chars = (value ?? string.Empty)
            .Select(c => char.IsLetterOrDigit(c) ? c : ' ')
            .ToArray();
        var slug = string.Join("_", new string(chars)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return string.IsNullOrEmpty(slug) ? fallback : slug;
    }
}
