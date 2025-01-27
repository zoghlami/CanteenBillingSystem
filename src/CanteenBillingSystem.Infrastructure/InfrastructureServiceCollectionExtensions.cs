using CanteenBillingSystem.Domain.Repositories;
using CanteenBillingSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CanteenBillingSystem.Application;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClientRepository, InMemoryClientRepository>();
        services.AddSingleton<IProductPriceRepository, InMemoryProductPriceRepository>();

        return services;
    }
}