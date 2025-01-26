using CanteenBillingSystem.Domain.Enums;

namespace CanteenBillingSystem.Domain.Entities
{
    public class MealItem
    {
        public ProductType Type { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public MealItem(ProductType type, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Type = type;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal GetTotalPrice()
        {
            return Quantity * UnitPrice;
        }
    }
}