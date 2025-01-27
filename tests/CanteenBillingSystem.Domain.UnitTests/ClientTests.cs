using System;
using AutoFixture;
using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Exceptions;
using CanteenBillingSystem.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CanteenBillingSystem.Domain.UnitTests
{
    public class ClientTests
    {
        private readonly Fixture _fixture;

        public ClientTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Create_Should_Create_Valid_Client()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var clientType = _fixture.Create<ClientType>();
            var initialBalance = _fixture.Create<decimal>();

            // Act
            var client = Client.Create(name, clientType, initialBalance);

            // Assert
            client.Should().NotBeNull();
            client.Name.Should().Be(name);
            client.ClientType.Should().Be(clientType);
            client.Balance.Value.Should().Be(initialBalance);
        }

        [Fact]
        public void Create_Should_Throw_Exception_When_Name_Is_Empty()
        {
            // Arrange
            var clientType = _fixture.Create<ClientType>();
            var initialBalance = _fixture.Create<decimal>();

            // Act
            Action act = () => Client.Create(string.Empty, clientType, initialBalance);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Name cannot be empty. (Parameter 'name')");
        }

        [Fact]
        public void Create_Should_Throw_Exception_When_InitialBalance_Is_Negative()
        {
            // Arrange
            var name = _fixture.Create<string>();
            var clientType = _fixture.Create<ClientType>();
            var initialBalance = -1m;

            // Act
            Action act = () => Client.Create(name, clientType, initialBalance);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("InitialBalance cannot be negative. (Parameter 'initialBalance')");
        }

        [Fact]
        public void Load_Should_Load_Client_With_Valid_Data()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var name = _fixture.Create<string>();
            var clientType = _fixture.Create<ClientType>();
            var balance = _fixture.Create<decimal>();

            // Act
            var client = Client.Load(clientId, name, clientType, balance);

            // Assert
            client.ClientId.Should().Be(clientId);
            client.Name.Should().Be(name);
            client.ClientType.Should().Be(clientType);
            client.Balance.Value.Should().Be(balance);
        }

        [Fact]
        public void Credit_Should_Increase_Balance()
        {
            // Arrange
            var client = CreateValidClient();
            var creditAmount = 50m;

            // Act
            client.Credit(creditAmount);

            // Assert
            client.Balance.Value.Should().Be(150m);
        }

        [Fact]
        public void Debit_Should_Decrease_Balance()
        {
            // Arrange
            var client = CreateValidClient();
            var debitAmount = 30m;

            // Act
            client.Debit(debitAmount);

            // Assert
            client.Balance.Value.Should().Be(70m);
        }

        [Fact]
        public void Debit_Should_Throw_Exception_When_Balance_Insufficient_And_No_Overdraft()
        {
            // Arrange
            var initialBalance = 50m;
            var client = Client.Create("Test User", ClientType.Contractor, initialBalance);
            var debitAmount = 100m;

            // Act
            Action act = () => client.Debit(debitAmount);

            // Assert
            act.Should().Throw<InsufficientBalanceException>()
                .WithMessage($"Insufficient balance. Current balance: {initialBalance:C}, attempted debit: {debitAmount:C}.");
        }

        [Fact]
        public void Debit_Should_Allow_Overdraft_For_Internal_Client()
        {
            // Arrange
            var client = Client.Create("Test User", ClientType.Internal, 50m);
            var debitAmount = 100m;

            // Act
            client.Debit(debitAmount);

            // Assert
            client.Balance.Value.Should().Be(-50m);
        }

        [Fact]
        public void Debit_Should_Allow_Overdraft_For_VIP_Client()
        {
            // Arrange
            var client = Client.Create("Test User", ClientType.VIP, 50m);
            var debitAmount = 100m;

            // Act
            client.Debit(debitAmount);

            // Assert
            client.Balance.Value.Should().Be(-50m);
        }

        [Theory]
        [InlineData(ClientType.Internal, 20, 7.5)]
        [InlineData(ClientType.Internal, 5, 5)]
        [InlineData(ClientType.Contractor, 20, 6)]
        [InlineData(ClientType.Contractor, 4, 4)]
        [InlineData(ClientType.VIP, 50, 50)]
        [InlineData(ClientType.Intern, 20, 10)]
        [InlineData(ClientType.Intern, 8, 8)]
        [InlineData(ClientType.Visitor, 20, 0)]
        public void GetEmployerContribution_ShouldReturnExpectedContribution(
            ClientType clientType, decimal total, decimal expectedContribution)
        {
            // Arrange
            var client = Client.Create("Test Client", clientType, 100);

            // Act
            var contribution = client.GetEmployerContribution(total);

            // Assert
            Assert.Equal(expectedContribution, contribution);
        }

        private Client CreateValidClient()
        {
            var name = "John Doe";
            var clientType = ClientType.Contractor;
            var balance = 100m;

            return Client.Create(name, clientType, balance);
        }
    }
}