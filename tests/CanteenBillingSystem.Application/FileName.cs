using CanteenBillingSystem.Application.UseCases.GetClients;
using CanteenBillingSystem.BuildersForTests;
using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Repositories;
using Moq;

namespace CanteenBillingSystem.Tests.Application.UseCases;

public class GetClientsUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsClients_WhenRepositoryHasClients()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Client>
            {
                new ClientBuilder().WithName("John Doe").Build(),
                new ClientBuilder().WithName("Jane Smith").Build()
            });

        var useCase = new GetClientsUseCase(mockRepository.Object);

        // Act
        var result = await useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Name == "John Doe");
        Assert.Contains(result, r => r.Name == "Jane Smith");
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsEmptyList_WhenRepositoryHasNoClients()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Client>());

        var useCase = new GetClientsUseCase(mockRepository.Object);

        // Act
        var result = await useCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}