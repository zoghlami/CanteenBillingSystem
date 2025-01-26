using CanteenBillingSystem.Application.Exceptions;
using CanteenBillingSystem.Domain.Exceptions;
using CanteenBillingSystem.Domain.Repositories;

namespace CanteenBillingSystem.Application.UseCases.CreditClientAccount
{
    public class CreditClientAccountUseCase : ICreditClientAccountUseCase
    {
        private readonly IClientRepository _clientRepository;

        public CreditClientAccountUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<decimal> ExecuteAsync(Guid clientId, decimal amount)
        {
            if (amount < 0)
                throw new InvalidAmountException(amount);
            var client = await _clientRepository.GetByIdAsync(clientId);
            if (client is null)
                throw new ClientNotFoundException(clientId);
            client.Credit(amount);
            await _clientRepository.UpdateBalanceAsync(client);

            return client.Balance.Value;
        }
    }
}