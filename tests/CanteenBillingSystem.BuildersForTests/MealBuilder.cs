using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;

namespace CanteenBillingSystem.BuildersForTests;

public class MealBuilder
{
    private readonly List<MealItem> _items = new();

    public MealBuilder WithEntree(int quantity = 1, decimal unitPrice = 3.00m)
    {
        _items.Add(new MealItem
        (
            type: ProductType.Entree,
            quantity: quantity,
            unitPrice: unitPrice
        ));
        return this;
    }

    public MealBuilder WithPlat(int quantity = 1, decimal unitPrice = 6.00m)
    {
        _items.Add(new MealItem
        (
             ProductType.Plat,
             quantity,
             unitPrice
        ));
        return this;
    }

    public MealBuilder WithDessert(int quantity = 1, decimal unitPrice = 3m)
    {
        _items.Add(new MealItem
        (
            ProductType.Dessert,
            quantity,
           unitPrice
        ));
        return this;
    }

    public MealBuilder WithPain(int quantity = 1, decimal unitPrice = 0.40m)
    {
        _items.Add(new MealItem
        (
          ProductType.Pain,
           quantity,
           unitPrice
       ));
        return this;
    }

    public MealBuilder WithBoisson(int quantity = 1, decimal unitPrice = 1.00m)
    {
        _items.Add(new MealItem
        (
          ProductType.Boisson,
           quantity,
           unitPrice
       ));
        return this;
    }

    public MealBuilder WithFromage(int quantity = 1, decimal unitPrice = 1.00m)
    {
        _items.Add(new MealItem
       (ProductType.Fromage,
           quantity,
           unitPrice
       ));
        return this;
    }

    public MealBuilder WithPetitSaladeBar(int quantity = 1, decimal unitPrice = 4.00m)
    {
        _items.Add(new MealItem
        (
          ProductType.PetitSaladeBar,
           quantity,
           unitPrice
       ));
        return this;
    }

    public MealBuilder WithGrandSaladeBar(int quantity = 1, decimal unitPrice = 6.00m)
    {
        _items.Add(new MealItem
        (
          ProductType.GrandSaladeBar,
           quantity,
           unitPrice
       ));
        return this;
    }

    public MealBuilder WithPortionFruit(int quantity = 1, decimal unitPrice = 1.00m)
    {
        _items.Add(new MealItem
        (
          ProductType.PortionFruit,
           quantity,
           unitPrice
       ));
        return this;
    }

    public MealBuilder WithMenu()
    {
        return this.WithPlat()
            .WithEntree()
            .WithDessert()
            .WithPain();
    }

    public Meal Build()
    {
        return new Meal(_items);
    }
}