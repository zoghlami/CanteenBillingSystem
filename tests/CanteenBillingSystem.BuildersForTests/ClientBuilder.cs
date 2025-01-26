using CanteenBillingSystem.Domain.Entities;
using CanteenBillingSystem.Domain.Enums;

namespace CanteenBillingSystem.BuildersForTests;

public class ClientBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "John Doe";
    private ClientType _type = ClientType.Intern;
    private decimal _balance = 100m;

    public ClientBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ClientBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ClientBuilder WithType(ClientType type)
    {
        _type = type;
        return this;
    }

    public ClientBuilder WithBalance(decimal balance)
    {
        _balance = balance;
        return this;
    }

    public Client Build()
    {
        return Client.Load(_id, _name, _type, _balance);
    }
}