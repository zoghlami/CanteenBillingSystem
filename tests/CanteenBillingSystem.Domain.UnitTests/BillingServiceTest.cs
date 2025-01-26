using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Services;
using CanteenBillingSystem.BuildersForTests;

namespace CanteenBillingSystem.Domain.UnitTests;

public class BillingServiceTests
{
    [Fact]
    public void GenerateTicket_ReturnsCorrectMealTicket_WhenValidInputsAreProvided()
    {
        // Arrange

        var meal = new MealBuilder()
            .WithMenu()
            .WithPetitSaladeBar()
            .WithFromage()
            .Build();

        var client = new ClientBuilder()
            .WithType(ClientType.Internal)
            .WithBalance(20).Build();

        // Act
        var ticket = BillingService.GenerateTicket(meal, client);

        // Assert
        Assert.Equal(15m, ticket.TotalCost);
        Assert.Equal(7.5m, ticket.EmployerContribution);
        Assert.Equal(7.5m, ticket.ClientShare);
        Assert.Equal(6, ticket.MealDetails.Count());
    }

    [Fact]
    public void GenerateTicket_CorrectlyHandlesVIPClient_WithFullEmployerContribution()
    {
        var meal = new MealBuilder()
           .WithMenu()
           .WithPetitSaladeBar()
           .WithFromage()
           .WithBoisson()
           .Build();

        var client = new ClientBuilder()
            .WithType(ClientType.VIP)
            .WithBalance(20)
            .Build();

        // Act
        var ticket = BillingService.GenerateTicket(meal, client);

        // Assert
        Assert.Equal(16m, ticket.TotalCost);
        Assert.Equal(16m, ticket.EmployerContribution);
        Assert.Equal(0m, ticket.ClientShare);
        Assert.Equal(7, ticket.MealDetails.Count());
    }

    [Fact]
    public void GenerateTicket_ReturnsCorrectValues_WhenNoEmployerContributionIsProvided()
    {
        // Arrange
        var meal = new MealBuilder()
           .WithMenu()
           .Build();

        var client = new ClientBuilder()
            .WithType(ClientType.Visitor)
            .WithBalance(20)
            .Build();

        // Act
        var ticket = BillingService.GenerateTicket(meal, client);

        // Assert
        Assert.Equal(10m, ticket.TotalCost);
        Assert.Equal(0m, ticket.EmployerContribution);
        Assert.Equal(10m, ticket.ClientShare);
        Assert.Equal(4, ticket.MealDetails.Count());
    }
}