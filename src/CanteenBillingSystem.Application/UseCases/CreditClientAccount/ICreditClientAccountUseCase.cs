namespace CanteenBillingSystem.Application.UseCases.CreditClientAccount
{
    public interface ICreditClientAccountUseCase
    {
        Task<decimal> ExecuteAsync(Guid clientId, decimal amount);
    }
}