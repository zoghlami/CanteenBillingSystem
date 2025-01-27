using CanteenBillingSystem.Application.Exceptions;
using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Repositories;
using CanteenBillingSystem.Domain.Services;

namespace CanteenBillingSystem.Application.UseCases.PayMealUseCase;

public class PayMealUseCase : IPayMealUseCase
{
    private readonly IProductPriceRepository _priceRepository;
    private readonly IClientRepository _clientRepository;

    public PayMealUseCase(IProductPriceRepository priceRepository, IClientRepository clientRepository)
    {
        _priceRepository = priceRepository;
        _clientRepository = clientRepository;
    }

    public async Task<MealPaymentResponse> ExecuteAsync(Guid clientId, List<MealItemRequest> items)
    {
        var client = await _clientRepository.GetByIdAsync(clientId);
        if (client is null)
            throw new ClientNotFoundException(clientId);

        var mealItems = MapToMealItems(items);
        var meal = new Meal(mealItems);
        var mealTicket = BillingService.GenerateTicket(meal, client);
        client.Debit(mealTicket.ClientShare);
        await _clientRepository.UpdateBalanceAsync(client);
        return new MealPaymentResponse
        {
            Success = true,
            Message = "Meal payment processed successfully.",
            Ticket = mealTicket,
        };
    }

    private List<MealItem> MapToMealItems(List<MealItemRequest> requests)
    {
        var mealItems = new List<MealItem>();

        foreach (var request in requests)
        {
            var price = _priceRepository.GetPrice(request.Type).UnitPrice;

            var mealItem = new MealItem(
                type: request.Type,
                quantity: request.Quantity,
                unitPrice: price
            );

            mealItems.Add(mealItem);
        }

        return mealItems;
    }
}