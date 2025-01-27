using CanteenBillingSystem.Domain.Exceptions;

namespace CanteenBillingSystem.Domain.ValueObjects
{
    public class Balance : IEquatable<Balance>
    {
        public decimal Value { get; }

        public Balance(decimal value)
        {
            Value = value;
        }

        public Balance Add(decimal amount)
        {
            if (amount < 0) throw new InvalidAmountException(amount);
            return new Balance(Value + amount);
        }

        public Balance Subtract(decimal amount)
        {
            if (amount < 0) throw new InvalidAmountException(amount);
            return new Balance(Value - amount);
        }

        public bool CanSubtract(decimal amount)
        {
            return amount >= 0 && Value >= amount;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Balance);
        }

        public bool Equals(Balance other)
        {
            return other != null && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value:C}";
        }
    }
}