using CanteenBillingSystem.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CanteenBillingSystem.Application.UseCases.PayMealUseCase;

public class MealItemRequest
{
    [SwaggerSchema("The type of product. Possible values: Entree, Plat, Dessert, Pain, Boisson, Fromage, PetitSaladeBar, GrandSaladeBar, PortionFruit.")]
    [Required]
    public ProductType Type { get; set; }

    [SwaggerSchema("The quantity of the product.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }
}