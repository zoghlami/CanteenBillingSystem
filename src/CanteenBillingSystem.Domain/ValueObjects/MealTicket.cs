using CanteenBillingSystem.Domain.Entities;

namespace CanteenBillingSystem.Domain.ValueObjects;

public class MealTicket
{
    public IEnumerable<MealItem> MealDetails { get; }
    public decimal TotalCost { get; }
    public decimal EmployerContribution { get; }
    public decimal ClientShare { get; }

    public MealTicket(IEnumerable<MealItem> mealDetails, decimal totalCost, decimal employerContribution, decimal clientShare)
    {
        MealDetails = mealDetails ?? throw new ArgumentNullException(nameof(mealDetails));
        TotalCost = totalCost;
        EmployerContribution = employerContribution;
        ClientShare = clientShare;
    }

    public override string ToString()
    {
        var details = string.Join(", ", MealDetails.Select(item => $"{item.Type} x{item.Quantity} ({item.GetTotalPrice():C})"));
        return $"Meal Details: {details}\n" +
               $"Total Cost: {TotalCost:C}\n" +
               $"Employer Contribution: {EmployerContribution:C}\n" +
               $"Client Share: {ClientShare:C}";
    }
}