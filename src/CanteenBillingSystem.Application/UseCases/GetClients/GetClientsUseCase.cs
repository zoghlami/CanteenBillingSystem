using CanteenBillingSystem.Domain.Repositories;

namespace CanteenBillingSystem.Application.UseCases.GetClients
{
    public class GetClientsUseCase : IGetClientsUseCase
    {
        private readonly IClientRepository _clientRepository;

        public GetClientsUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<GetClientResponse>> ExecuteAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(client => new GetClientResponse
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ClientType = client.ClientType.ToString(),
                Balance = client.Balance.Value
            });
        }
    }
}