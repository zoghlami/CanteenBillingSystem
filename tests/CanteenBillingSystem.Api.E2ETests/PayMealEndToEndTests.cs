using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using CanteenBillingSystem.Application.UseCases.PayMealUseCase;
using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using CanteenBillingSystem.API.Middlewares;

namespace CanteenBillingSystem.Api.E2ETests;

public class PayMealEndToEndTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IClientRepository _clientRepository;

    public PayMealEndToEndTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _clientRepository = factory.Services.GetRequiredService<IClientRepository>();
    }

    [Fact]
    public async Task PayMeal_EndToEnd_ShouldDeductBalanceAndReturnMealTicket_WhenSuccessful()
    {
        // Arrange
        var clientId = "11111111-1111-1111-1111-111111111111"; // Internal user with sufficient balance
        var client = await _clientRepository.GetByIdAsync(Guid.Parse(clientId));
        var initialBalance = client!.Balance.Value;

        var mealItems = new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Plat, Quantity = 1 },
             new MealItemRequest { Type = ProductType.Dessert, Quantity = 1 },
              new MealItemRequest { Type = ProductType.Pain, Quantity = 1 }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/v1/clients/{clientId}/pay-meal", mealItems);

        // Assert - HTTP Response
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MealPaymentResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Ticket.MealDetails.Should().HaveCount(4);
        result.Ticket.TotalCost.Should().Be(10m); // Fixed price for the meal

        // Assert - Repository State

        var updatedClient = await _clientRepository.GetByIdAsync(Guid.Parse(clientId));
        updatedClient!.Balance.Value.Should().Be(initialBalance - 2.5m); // Deduct client portion
    }

    [Fact]
    public async Task PayMeal_EndToEnd_ShouldReturnBadRequest_WhenInsufficientBalance()
    {
        // Arrange
        var clientId = "22111111-1111-1111-1111-111111111112"; // Contractor user with no balance
        var mealItems = new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Plat, Quantity = 1 }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/v1/clients/{clientId}/pay-meal", mealItems);

        // Assert - HTTP Response
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.PaymentRequired);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.Message.Should().Contain("Insufficient balance");

        // Assert - Repository State (No change)
        var client = await _clientRepository.GetByIdAsync(Guid.Parse(clientId));
        client!.Balance.Value.Should().Be(0m); // No balance deducted
    }

    [Fact]
    public async Task PayMeal_EndToEnd_ShouldReturnNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = new Guid(); // Nonexistent client
        var mealItems = new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Plat, Quantity = 1 }
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/v1/clients/{clientId}/pay-meal", mealItems);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.Message.Should().Contain($"Client with ID {clientId} not found.");
    }
}