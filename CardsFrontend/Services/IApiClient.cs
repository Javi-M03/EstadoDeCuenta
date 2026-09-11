using CardsFrontend.Models;

namespace CardsFrontend.Services;

public interface IApiClient
{
    // Clients
    Task<List<Client>> GetClientsAsync();
    Task<Client?> GetClientAsync(int clientId);
    Task<Client?> CreateClientAsync(Client client);

    // Cards
    Task<List<Card>> GetClientCardsAsync(int clientId);
    Task<Card?> CreateCardAsync(int clientId, Card card);
    Task<List<Card>> GetCardsAsync();
    Task<Card?> GetCardAsync(int cardId);
    Task<List<Movement>> GetCardMovementsAsync(
        int cardId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? movementType = null);
    Task<Statement?> GetCardStatementAsync(int cardId);

    // Movements
    Task<Movement?> CreateMovementAsync(int cardId, MovementCreateRequest request);
    Task<List<Movement>> GetMovementsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<Movement?> GetMovementAsync(int movementId);

    // Settings
    Task<AccountStatementSettings?> GetAccountStatementSettingsAsync();
    Task<AccountStatementSettings?> UpdateAccountStatementSettingsAsync(AccountStatementSettings settings);

    // Health
    Task<ApiHealth> GetHealthAsync();
}
