using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;
using CanteenBillingSystem.Domain.Repositories;
using System.Collections.Concurrent;

namespace CanteenBillingSystem.Infrastructure.Repositories;

public class InMemoryClientRepository : IClientRepository
{
    private static readonly ConcurrentDictionary<Guid, ClientDto> Clients = new();

    public InMemoryClientRepository()
    {
        if (Clients.Count == 0)
            SeedClients();
    }

    public Task<Client?> GetByIdAsync(Guid clientId)
    {
        Clients.TryGetValue(clientId, out var result);

        return Task.FromResult(result != null
               ? Client.Load(result.ClientId, result.Name, result.ClientType, result.Balance)
               : null);
    }

    public Task UpdateBalanceAsync(Client client)
    {
        if (Clients.TryGetValue(client.ClientId, out var result))
        {
            result.Balance = client.Balance.Value;
            return Task.CompletedTask;
        }

        throw new KeyNotFoundException($"Client with ID {client.ClientId} not found.");
    }

    public Task<IEnumerable<Client>> GetAllAsync()
    {
        var clients = Clients.Values.Select(dto =>
        {
            var client = Client.Load(
                dto.ClientId,
                dto.Name,
                dto.ClientType,
                dto.Balance
            );
            return client;
        }
    );
        return Task.FromResult(clients);
    }

    // Méthode pour initialiser quelques données de test
    public static void SeedClients()
    {
        Clients.TryAdd(Guid.Parse("11111111-1111-1111-1111-111111111111"), new ClientDto
        {
            ClientId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Internal User",
            ClientType = ClientType.Internal,
            Balance = 100m
        });
        Clients.TryAdd(Guid.Parse("12111111-1111-1111-1111-111111111111"), new ClientDto
        {
            ClientId = Guid.Parse("12111111-1111-1111-1111-111111111111"),
            Name = "Internal User",
            ClientType = ClientType.Internal,
            Balance = 0m
        });

        Clients.TryAdd(Guid.Parse("21111111-1111-1111-1111-111111111112"), new ClientDto
        {
            ClientId = Guid.Parse("21111111-1111-1111-1111-111111111112"),
            Name = "Contractor User",
            ClientType = ClientType.Contractor,
            Balance = 200m
        });
        Clients.TryAdd(Guid.Parse("22111111-1111-1111-1111-111111111112"), new ClientDto
        {
            ClientId = Guid.Parse("22111111-1111-1111-1111-111111111112"),
            Name = "Contractor User",
            ClientType = ClientType.Contractor,
            Balance = 0m
        });

        Clients.TryAdd(Guid.Parse("31111111-1111-1111-1111-111111111113"), new ClientDto
        {
            ClientId = Guid.Parse("31111111-1111-1111-1111-111111111113"),
            Name = "VIP User",
            ClientType = ClientType.VIP,
            Balance = 1000m
        });
        Clients.TryAdd(Guid.Parse("32111111-1111-1111-1111-111111111113"), new ClientDto
        {
            ClientId = Guid.Parse("32111111-1111-1111-1111-111111111113"),
            Name = "VIP User",
            ClientType = ClientType.VIP,
            Balance = 0m
        });
        Clients.TryAdd(Guid.Parse("41111111-1111-1111-1111-111111111114"), new ClientDto
        {
            ClientId = Guid.Parse("41111111-1111-1111-1111-111111111114"),
            Name = "Intern User",
            ClientType = ClientType.Intern,
            Balance = 1000m
        });
        Clients.TryAdd(Guid.Parse("51111111-1111-1111-1111-111111111115"), new ClientDto
        {
            ClientId = Guid.Parse("51111111-1111-1111-1111-111111111115"),
            Name = "Visitor User",
            ClientType = ClientType.Visitor,
            Balance = 1000m
        });
    }
}

// DTO représentant un client
internal record ClientDto
{
    public Guid ClientId { get; set; }
    public string Name { get; set; }
    public ClientType ClientType { get; set; }
    public decimal Balance { get; set; }
}