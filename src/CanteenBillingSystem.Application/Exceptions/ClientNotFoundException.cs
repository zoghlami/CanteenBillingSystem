namespace CanteenBillingSystem.Application.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(Guid clientId)
            : base($"Client with ID {clientId} not found.") { }
    }
}