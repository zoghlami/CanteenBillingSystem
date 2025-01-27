namespace CanteenBillingSystem.Domain.Exceptions
{
    public class InvalidAmountException : DomainException
    {
        public InvalidAmountException(decimal amount)
            : base($"The amount {amount} is invalid. It must be greater than zero.") { }
    }
}