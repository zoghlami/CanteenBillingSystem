using CanteenBillingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CanteenBillingSystem.Domain.Entities
{
    public class Meal
    {
        private const decimal FIXED_PRICE = 10m;
        private readonly List<MealItem> _items;

        public IReadOnlyCollection<MealItem> Items => _items.AsReadOnly();

        public Meal(List<MealItem> items)
        {
            _items = items ?? new List<MealItem>();
        }

        public void AddItem(MealItem item)
        {
            _items.Add(item);
        }

        public decimal GetTotal()
        {
            if (IsMenu())
            {
                decimal supplementTotal = CalculateSupplements();
                return FIXED_PRICE + supplementTotal;
            }

            return _items.Sum(item => item.GetTotalPrice());
        }

        private decimal CalculateSupplements()
        {
            return _items
                .GroupBy(item => item.Type)
                .Sum(group =>
                {
                    var totalQuantity = group.Sum(item => item.Quantity);
                    var baseQuantity = IsBaseType(group.Key) ? 1 : 0;
                    var supplementQuantity = Math.Max(0, totalQuantity - baseQuantity);

                    return supplementQuantity * group.First().UnitPrice;
                });
        }

        private bool IsMenu()
        {
            return _items.Count(i => i.Type == ProductType.Entree) >= 1 &&
                    _items.Count(i => i.Type == ProductType.Plat) >= 1 &&
                    _items.Count(i => i.Type == ProductType.Dessert) >= 1 &&
                    _items.Count(i => i.Type == ProductType.Pain) >= 1;
        }

        private bool IsBaseType(ProductType type)
        {
            return type == ProductType.Entree ||
                   type == ProductType.Plat ||
                   type == ProductType.Dessert ||
                   type == ProductType.Pain;
        }
    }
}