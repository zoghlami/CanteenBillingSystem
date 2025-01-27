using CanteenBillingSystem.API.Controllers.v1;
using CanteenBillingSystem.Application.Exceptions;
using CanteenBillingSystem.Application.UseCases.CreditClientAccount;
using CanteenBillingSystem.Application.UseCases.GetClients;
using CanteenBillingSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CanteenBillingSystem.Api.UnitTests;

public class ClientControllerTests
{
    private readonly Mock<ICreditClientAccountUseCase> _creditClientuseCaseMock;
    private readonly Mock<IGetClientsUseCase> _getClientUseCaseMock;
    private readonly ClientController _controller;

    public ClientControllerTests()
    {
        _creditClientuseCaseMock = new Mock<ICreditClientAccountUseCase>();
        _getClientUseCaseMock = new Mock<IGetClientsUseCase>();
        _controller = new ClientController(_creditClientuseCaseMock.Object, _getClientUseCaseMock.Object);
    }

    [Fact]
    public async Task CreditAccount_ShouldReturnOk_WhenValidRequest()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var amount = 100m;
        var newBalance = 200m;

        _creditClientuseCaseMock
            .Setup(useCase => useCase.ExecuteAsync(clientId, amount))
            .ReturnsAsync(newBalance);

        var request = new CreditAccountRequest
        {
            Amount = amount
        };

        // Act
        var result = await _controller.CreditAccount(clientId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<CreditClientAccountResponse>(okResult.Value);

        Assert.True(response.Success);
        Assert.Equal("Account credited successfully.", response.Message);
        Assert.Equal(newBalance, response.NewBalance);
    }

    [Fact]
    public async Task CreditAccount_ShouldReturnNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var amount = 100m;

        _creditClientuseCaseMock
            .Setup(useCase => useCase.ExecuteAsync(clientId, amount))
            .ThrowsAsync(new ClientNotFoundException(clientId));

        var request = new CreditAccountRequest
        {
            Amount = amount
        };

        //Act
        await Assert.ThrowsAsync<ClientNotFoundException>(() => _controller.CreditAccount(clientId, request));
    }

    [Fact]
    public async Task CreditAccount_ShouldReturnBadRequest_WhenInvalidAmount()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var amount = -10m;

        _creditClientuseCaseMock
            .Setup(useCase => useCase.ExecuteAsync(clientId, amount))
            .ThrowsAsync(new InvalidAmountException(amount));

        var request = new CreditAccountRequest
        {
            Amount = amount
        };

        //Act & Assert
        await Assert.ThrowsAsync<InvalidAmountException>(() => _controller.CreditAccount(clientId, request));
    }
}