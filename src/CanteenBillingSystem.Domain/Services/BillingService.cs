using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.ValueObjects;

namespace CanteenBillingSystem.Domain.Services;

public static class BillingService
{
    public static MealTicket GenerateTicket(Meal meal, Client client)
    {
        decimal mealCost = meal.GetTotal();

        decimal employerContribution = client.GetEmployerContribution(mealCost);

        decimal clientShare = mealCost - employerContribution;

        return new MealTicket(
            mealDetails: meal.Items,
            totalCost: mealCost,
            employerContribution: employerContribution,
            clientShare: clientShare
        );
    }
}