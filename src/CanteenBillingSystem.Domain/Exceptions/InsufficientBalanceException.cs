namespace CanteenBillingSystem.Domain.Exceptions
{
    public class InsufficientBalanceException : DomainException
    {
        public InsufficientBalanceException(decimal balance, decimal amount)
            : base($"Insufficient balance. Current balance: {balance:C}, attempted debit: {amount:C}.")
        {
        }
    }
}