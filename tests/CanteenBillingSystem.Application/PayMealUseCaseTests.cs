using CanteenBillingSystem.Application.Exceptions;
using CanteenBillingSystem.Application.UseCases.PayMealUseCase;
using CanteenBillingSystem.BuildersForTests;
using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Exceptions;
using CanteenBillingSystem.Domain.Repositories;
using CanteenBillingSystem.Domain.ValueObjects;
using Moq;

namespace CanteenBillingSystem.Application.UnitTests;

public class PayMealUseCaseTests
{
    private readonly Mock<IProductPriceRepository> _priceRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly PayMealUseCase _payMealUseCase;

    public PayMealUseCaseTests()
    {
        _priceRepositoryMock = new Mock<IProductPriceRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _payMealUseCase = new PayMealUseCase(_priceRepositoryMock.Object, _clientRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessPaymentSuccessfully_WhenClientAndItemsAreValid()
    {
        // Arrange
        var client = new ClientBuilder()
          .WithType(ClientType.Intern)
          .WithBalance(100m)
          .Build();
        var mealItemRequests = new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Plat, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Dessert, Quantity = 1 }
        };

        _clientRepositoryMock
            .Setup(repo => repo.GetByIdAsync(client.ClientId))
            .ReturnsAsync(client);

        _priceRepositoryMock
            .Setup(repo => repo.GetPrice(It.IsAny<ProductType>()))
            .Returns((ProductType type) => new ProductPrice(type, type switch
            {
                ProductType.Entree => 3m,
                ProductType.Plat => 6m,
                ProductType.Dessert => 3m,
                _ => throw new ArgumentException("Invalid product type")
            }));

        // Act
        var response = await _payMealUseCase.ExecuteAsync(client.ClientId, mealItemRequests);

        // Assert
        Assert.True(response.Success);
        Assert.Equal(12m, response.Ticket.TotalCost);
        Assert.Equal(2m, response.Ticket.ClientShare);
        _clientRepositoryMock.Verify(repo => repo.GetByIdAsync(client.ClientId), Times.Once);
        _clientRepositoryMock.Verify(repo => repo.UpdateBalanceAsync(client), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var mealItemRequests = new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 }
        };

        _clientRepositoryMock
            .Setup(repo => repo.GetByIdAsync(clientId))
            .ReturnsAsync((Client)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClientNotFoundException>(() => _payMealUseCase.ExecuteAsync(clientId, mealItemRequests));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInsufficientBalanceException_WhenClientBalanceIsLow()
    {
        // Arrange

        var client = new ClientBuilder()
            .WithBalance(2m) // Low balance
            .WithType(ClientType.Visitor)
            .Build();
        var clientId = client.ClientId;
        var mealItemRequests = new List<MealItemRequest>
        {
            new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
            new MealItemRequest { Type = ProductType.Plat, Quantity = 1 }
        };

        _clientRepositoryMock
            .Setup(repo => repo.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        _priceRepositoryMock
            .Setup(repo => repo.GetPrice(It.IsAny<ProductType>()))
            .Returns((ProductType type) => new ProductPrice(type, type switch
            {
                ProductType.Entree => 3m,
                ProductType.Plat => 6m,
                _ => throw new ArgumentException("Invalid product type")
            }));

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientBalanceException>(() => _payMealUseCase.ExecuteAsync(clientId, mealItemRequests));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessSuccessfully_WhenVipClientWithLowBalanceAndBigMeal()
    {
        // Arrange
        var client = new ClientBuilder()
            .WithBalance(0m) // Very low balance
            .WithType(ClientType.VIP) // VIP clients have 100% employer contribution
            .Build();
        var clientId = client.ClientId;

        var mealItemRequests = new List<MealItemRequest>
    {
        new MealItemRequest { Type = ProductType.Entree, Quantity = 1 },
        new MealItemRequest { Type = ProductType.Plat, Quantity = 2 },
        new MealItemRequest { Type = ProductType.Dessert, Quantity = 3 },
        new MealItemRequest { Type = ProductType.Pain, Quantity = 1 }
    };

        _clientRepositoryMock
            .Setup(repo => repo.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        _priceRepositoryMock
            .Setup(repo => repo.GetPrice(It.IsAny<ProductType>()))
            .Returns((ProductType type) => new ProductPrice(type, type switch
            {
                ProductType.Entree => 3m,
                ProductType.Plat => 6m,
                ProductType.Dessert => 3m,
                ProductType.Pain => 0.4m,
                _ => throw new ArgumentException("Invalid product type")
            }));

        // Act
        var response = await _payMealUseCase.ExecuteAsync(clientId, mealItemRequests);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Meal payment processed successfully.", response.Message);
        Assert.Equal(0m, response.Ticket.ClientShare); // VIP clients pay nothing
        Assert.Equal(0m, client.Balance.Value); // Balance remains unchanged
        Assert.NotNull(response.Ticket);
        Assert.Equal(22m, response.Ticket.TotalCost); // Total price of the meal
        _clientRepositoryMock.Verify(repo => repo.GetByIdAsync(clientId), Times.Once);
    }
}