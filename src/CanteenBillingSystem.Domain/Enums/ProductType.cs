using System.Text.Json.Serialization;

namespace CanteenBillingSystem.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductType
{
    Entree,
    Plat,
    Dessert,
    Pain,
    Boisson,
    Fromage,
    PetitSaladeBar,
    GrandSaladeBar,
    PortionFruit
}