using CanteenBillingSystem.Domain.ValueObjects;

namespace CanteenBillingSystem.Application.UseCases.PayMealUseCase;

public record MealPaymentResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public MealTicket Ticket { get; set; }
}