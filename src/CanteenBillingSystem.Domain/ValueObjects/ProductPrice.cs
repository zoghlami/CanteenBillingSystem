using CanteenBillingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenBillingSystem.Domain.ValueObjects
{
    using System;

    public class ProductPrice : IEquatable<ProductPrice>
    {
        public ProductType Type { get; }
        public decimal UnitPrice { get; }

        public ProductPrice(ProductType type, decimal unitPrice)
        {
            if (unitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

            Type = type;
            UnitPrice = unitPrice;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProductPrice);
        }

        public bool Equals(ProductPrice other)
        {
            return other is not null &&
                   Type == other.Type &&
                   UnitPrice == other.UnitPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, UnitPrice);
        }

        public static bool operator ==(ProductPrice left, ProductPrice right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductPrice left, ProductPrice right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{Type}: {UnitPrice:C}";
        }
    }
}