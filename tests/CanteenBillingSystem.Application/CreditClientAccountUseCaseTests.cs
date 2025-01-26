using System;
using System.Threading.Tasks;
using CanteenBillingSystem.Application.Exceptions;
using CanteenBillingSystem.Application.UseCases.CreditClientAccount;
using CanteenBillingSystem.BuildersForTests;
using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Exceptions;
using CanteenBillingSystem.Domain.Repositories;
using Moq;
using Xunit;

namespace CanteenBillingSystem.Application.UnitTests;

public class CreditClientAccountUseCaseTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly CreditClientAccountUseCase _useCase;

    public CreditClientAccountUseCaseTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _useCase = new CreditClientAccountUseCase(_clientRepositoryMock.Object);
    }

    [Theory]
    [InlineData(100, 50, 150)]
    [InlineData(200, 100, 300)]
    [InlineData(0, 50, 50)]
    public async Task ExecuteAsync_ShouldCreditClient_WhenValidInput(decimal initialBalance, decimal amountToCredit, decimal expectedBalance)
    {
        // Arrange
        var clientId = Guid.NewGuid();

        var client = new ClientBuilder()
            .WithBalance(initialBalance)
            .WithId(clientId)
            .Build();

        _clientRepositoryMock
            .Setup(repo => repo.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        // Act
        var newBalance = await _useCase.ExecuteAsync(clientId, amountToCredit);

        // Assert
        Assert.Equal(expectedBalance, newBalance);
        _clientRepositoryMock.Verify(repo => repo.UpdateBalanceAsync(client), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowClientNotFoundException_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var amountToCredit = 50m;

        _clientRepositoryMock
            .Setup(repo => repo.GetByIdAsync(clientId))
            .ReturnsAsync((Client)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClientNotFoundException>(() => _useCase.ExecuteAsync(clientId, amountToCredit));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidAmountException_WhenAmountIsNegative()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var amountToCredit = -10m;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidAmountException>(() => _useCase.ExecuteAsync(clientId, amountToCredit));
    }
}