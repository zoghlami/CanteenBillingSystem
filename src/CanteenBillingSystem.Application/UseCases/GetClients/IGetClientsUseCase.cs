namespace CanteenBillingSystem.Application.UseCases.GetClients
{
    public interface IGetClientsUseCase
    {
        Task<IEnumerable<GetClientResponse>> ExecuteAsync();
    }
}