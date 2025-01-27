using CanteenBillingSystem.Application.UseCases.PayMealUseCase;
using CanteenBillingSystem.Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace CanteenBillingSystem.API.Swagger
{
    public class MealItemRequestExample : IExamplesProvider<List<MealItemRequest>>
    {
        public List<MealItemRequest> GetExamples()
        {
            return new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Plat, Quantity = 2 },
            new MealItemRequest { Type = ProductType.Dessert, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Pain, Quantity = 1 }
        };
        }
    }
}