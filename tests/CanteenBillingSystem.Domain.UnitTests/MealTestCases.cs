using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;
using System.Collections;

namespace CanteenBillingSystem.Domain.UnitTests
{
    public class MealTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // Case 1: Fixed price meal (1 Entrée, 1 Plat, 1 Dessert, 1 Pain)
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m)
            },
            10m // Expected total (fixed price)
            };

            // Case 2: Fixed price meal + 1 Boisson
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m),
                new MealItem(ProductType.Boisson, 1, 1m)
            },
            11m // Fixed price + 1 Boisson
            };

            // Case 3: Fixed price meal + 1 Fromage
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m),
                new MealItem(ProductType.Fromage, 1, 1m)
            },
            11m // Fixed price + 1 Fromage
            };

            // Case 4: Meal without Pain (not fixed price)
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m)
            },
            12m // Not a fixed price, total = sum of items
            };

            // Case 5: Meal with 2 Desserts (1 as a supplement)
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 2, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m)
            },
            13m // Fixed price + 1 Dessert supplement
            };

            // Case 6: Fixed price meal + Petit Salade Bar
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m),
                new MealItem(ProductType.PetitSaladeBar, 1, 4m)
            },
            14m // Fixed price + Petit Salade Bar
            };

            // Case 7: Fixed price meal + Grand Salade Bar
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m),
                new MealItem(ProductType.GrandSaladeBar, 1, 6m)
            },
            16m // Fixed price + Grand Salade Bar
            };

            // Case 8: Only supplements (no fixed price)
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Boisson, 1, 1m),
                new MealItem(ProductType.Fromage, 1, 1m),
                new MealItem(ProductType.PetitSaladeBar, 1, 4m)
            },
            6m // Sum of supplements
            };

            // Case 9: Meal with Portion de Fruit
            yield return new object[]
            {
            new List<MealItem>
            {
                new MealItem(ProductType.Entree, 1, 3m),
                new MealItem(ProductType.Plat, 1, 6m),
                new MealItem(ProductType.Dessert, 1, 3m),
                new MealItem(ProductType.Pain, 1, 0.4m),
                new MealItem(ProductType.PortionFruit, 1, 1m)
            },
            11m // Fixed price + Portion de Fruit
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}