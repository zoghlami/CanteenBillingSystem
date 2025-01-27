using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Repositories;
using CanteenBillingSystem.Domain.ValueObjects;

namespace CanteenBillingSystem.Infrastructure.Repositories;

public class InMemoryProductPriceRepository : IProductPriceRepository
{
    private readonly List<ProductPrice> _productPrices;

    public InMemoryProductPriceRepository()
    {
        _productPrices = new List<ProductPrice>
    {
        new ProductPrice(ProductType.Boisson, 1.0m),
        new ProductPrice(ProductType.Fromage, 1.0m),
        new ProductPrice(ProductType.Pain, 0.40m),
        new ProductPrice(ProductType.PetitSaladeBar, 4.0m),
        new ProductPrice(ProductType.GrandSaladeBar, 6.0m),
        new ProductPrice(ProductType.PortionFruit, 1.0m),
        new ProductPrice(ProductType.Entree, 3.0m),
        new ProductPrice(ProductType.Plat, 6.0m),
        new ProductPrice(ProductType.Dessert, 3.0m)
    };
    }

    public ProductPrice GetPrice(ProductType type)
    {
        return _productPrices.FirstOrDefault(p => p.Type == type)
               ?? throw new KeyNotFoundException($"Price for product type '{type}' not found.");
    }

    public IEnumerable<ProductPrice> GetAllPrices()
    {
        return _productPrices;
    }
}