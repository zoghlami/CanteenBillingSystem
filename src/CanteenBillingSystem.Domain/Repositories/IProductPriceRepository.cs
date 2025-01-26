using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.ValueObjects;

namespace CanteenBillingSystem.Domain.Repositories;

public interface IProductPriceRepository
{
    ProductPrice GetPrice(ProductType type);

    IEnumerable<ProductPrice> GetAllPrices();
}