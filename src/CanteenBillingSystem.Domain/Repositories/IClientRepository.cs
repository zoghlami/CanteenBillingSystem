using CanteenBillingSystem.Domain.Entities;

namespace CanteenBillingSystem.Domain.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();

        Task<Client?> GetByIdAsync(Guid clientId);

        Task UpdateBalanceAsync(Client client);
    }
}