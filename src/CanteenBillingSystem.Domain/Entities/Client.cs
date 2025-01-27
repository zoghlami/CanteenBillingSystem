using CanteenBillingSystem.Domain.Constants;
using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Exceptions;
using CanteenBillingSystem.Domain.ValueObjects;

namespace CanteenBillingSystem.Domain.Entities
{
    public class Client
    {
        public Guid ClientId { get; private set; }
        public string Name { get; private set; }
        public ClientType ClientType { get; private set; }
        public Balance Balance { get; private set; }

        // Constructor
        private Client(Guid clientId, string name, ClientType clientType, decimal balance)
        {
            ClientId = clientId;
            Name = name;
            ClientType = clientType;
            Balance = new Balance(balance);
        }

        public static Client Create(string name, ClientType clientType, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (initialBalance < 0)
                throw new ArgumentException("InitialBalance cannot be negative.", nameof(initialBalance));

            return new Client(new Guid(), name, clientType, initialBalance);
        }

        public static Client Load(Guid clientId, string name, ClientType clientType, decimal balance)
        {
            return new Client(clientId, name, clientType, balance);
        }

        // Methods
        public void Credit(decimal amount)
        {
            Balance = Balance.Add(amount);
        }

        public void Debit(decimal amount)
        {
            if (!CanOverdraft() && !Balance.CanSubtract(amount))
            {
                throw new InsufficientBalanceException(Balance.Value, amount);
            }

            Balance = Balance.Subtract(amount);
        }

        public bool CanOverdraft()
        {
            return ClientType == ClientType.Internal || ClientType == ClientType.VIP;
        }

        public decimal GetEmployerContribution(decimal total)
        {
            return ClientType switch
            {
                ClientType.Internal => Math.Min(EmployerContributionPolicy.INTERNAL_MAX, total),
                ClientType.Contractor => Math.Min(EmployerContributionPolicy.CONTRACTOR_MAX, total),
                ClientType.VIP => total * EmployerContributionPolicy.VIP_PERCENTAGE,
                ClientType.Intern => Math.Min(EmployerContributionPolicy.INTERN_MAX, total),
                ClientType.Visitor => EmployerContributionPolicy.VISITOR_MAX,
                _ => EmployerContributionPolicy.DEFAULT_CONTRIBUTION
            };
        }

        public override string ToString()
        {
            return $"{Name} (ID: {ClientId}, Type: {ClientType}, Balance: {Balance})";
        }
    }
}