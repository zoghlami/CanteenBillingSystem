namespace CanteenBillingSystem.Application.UseCases.PayMealUseCase;

public interface IPayMealUseCase
{
    Task<MealPaymentResponse> ExecuteAsync(Guid clientId, List<MealItemRequest> items);
}