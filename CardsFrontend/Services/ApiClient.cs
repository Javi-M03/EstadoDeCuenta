using System.Net.Http.Json;
using CardsFrontend.Models;

namespace CardsFrontend.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CardsApi");
    }

    // ---------- Clients ----------

    public async Task<List<Client>> GetClientsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Client>>("api/clients");
        return result ?? new List<Client>();
    }

    public async Task<Client?> GetClientAsync(int clientId)
    {
        return await _httpClient.GetFromJsonAsync<Client>($"api/clients/{clientId}");
    }

    public async Task<Client?> CreateClientAsync(Client client)
    {
        var response = await _httpClient.PostAsJsonAsync("api/clients", client);
        await EnsureSuccessOrThrowAsync(response);
        return await response.Content.ReadFromJsonAsync<Client>();
    }

    // ---------- Cards ----------

    public async Task<List<Card>> GetClientCardsAsync(int clientId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<Card>>($"api/clients/{clientId}/cards");
        return result ?? new List<Card>();
    }

    public async Task<Card?> CreateCardAsync(int clientId, Card card)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/clients/{clientId}/cards", card);
        await EnsureSuccessOrThrowAsync(response);
        return await response.Content.ReadFromJsonAsync<Card>();
    }

    public async Task<List<Card>> GetCardsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Card>>("api/cards");
        return result ?? new List<Card>();
    }

    public async Task<Card?> GetCardAsync(int cardId)
    {
        return await _httpClient.GetFromJsonAsync<Card>($"api/cards/{cardId}");
    }

    public async Task<List<Movement>> GetCardMovementsAsync(
        int cardId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? movementType = null)
    {
        var query = new List<string>();
        if (fromDate.HasValue)
            query.Add($"fromDate={Uri.EscapeDataString(fromDate.Value.ToString("s"))}");
        if (toDate.HasValue)
            query.Add($"toDate={Uri.EscapeDataString(toDate.Value.ToString("s"))}");
        if (!string.IsNullOrWhiteSpace(movementType))
            query.Add($"movementType={Uri.EscapeDataString(movementType)}");

        var url = $"api/cards/{cardId}/movements";
        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        var result = await _httpClient.GetFromJsonAsync<List<Movement>>(url);
        return result ?? new List<Movement>();
    }

    public async Task<Statement?> GetCardStatementAsync(int cardId)
    {
        return await _httpClient.GetFromJsonAsync<Statement>($"api/cards/{cardId}/statement");
    }

    // ---------- Movements ----------

    public async Task<Movement?> CreateMovementAsync(int cardId, MovementCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/cards/{cardId}/movements", request);
        await EnsureSuccessOrThrowAsync(response);
        return await response.Content.ReadFromJsonAsync<Movement>();
    }

    /// <summary>
    /// If the response is not successful, reads the API's <see cref="ApiErrorResponse"/>
    /// body and throws an <see cref="ApiException"/> carrying that message, so pages can
    /// show it to the user. Falls back to the raw body / status code when the response
    /// isn't the expected JSON shape.
    /// </summary>
    private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var statusCode = (int)response.StatusCode;
        string message;
        try
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            message = string.IsNullOrWhiteSpace(error?.Message)
                ? $"The API returned status code {statusCode}."
                : error!.Message;
        }
        catch
        {
            var raw = await response.Content.ReadAsStringAsync();
            message = string.IsNullOrWhiteSpace(raw)
                ? $"The API returned status code {statusCode}."
                : raw;
        }

        throw new ApiException(statusCode, message);
    }

    public async Task<List<Movement>> GetMovementsAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = new List<string>();
        if (fromDate.HasValue)
            query.Add($"fromDate={Uri.EscapeDataString(fromDate.Value.ToString("s"))}");
        if (toDate.HasValue)
            query.Add($"toDate={Uri.EscapeDataString(toDate.Value.ToString("s"))}");

        var url = "api/movements";
        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        var result = await _httpClient.GetFromJsonAsync<List<Movement>>(url);
        return result ?? new List<Movement>();
    }

    public async Task<Movement?> GetMovementAsync(int movementId)
    {
        return await _httpClient.GetFromJsonAsync<Movement>($"api/movements/{movementId}");
    }

    // ---------- Settings ----------

    public async Task<AccountStatementSettings?> GetAccountStatementSettingsAsync()
    {
        return await _httpClient.GetFromJsonAsync<AccountStatementSettings>("api/settings/account-statement");
    }

    public async Task<AccountStatementSettings?> UpdateAccountStatementSettingsAsync(AccountStatementSettings settings)
    {
        var response = await _httpClient.PutAsJsonAsync("api/settings/account-statement", settings);
        await EnsureSuccessOrThrowAsync(response);
        return await response.Content.ReadFromJsonAsync<AccountStatementSettings>();
    }

    // ---------- Health ----------

    public async Task<ApiHealth> GetHealthAsync()
    {
        try
        {
            // The API exposes the health check at the root ("/health"),
            // returning 200 "Healthy" or 503 "Unhealthy".
            var response = await _httpClient.GetAsync("health");
            var body = (await response.Content.ReadAsStringAsync()).Trim();

            return new ApiHealth
            {
                IsHealthy = response.IsSuccessStatusCode,
                Status = string.IsNullOrWhiteSpace(body)
                    ? (response.IsSuccessStatusCode ? "Healthy" : "Unhealthy")
                    : body
            };
        }
        catch (Exception ex)
        {
            return new ApiHealth
            {
                IsHealthy = false,
                Status = "No se pudo conectar con la API. " + ex.Message
            };
        }
    }
}
