using CanteenBillingSystem.Application.UseCases.CreditClientAccount;
using CanteenBillingSystem.Application.UseCases.GetClients;
using CanteenBillingSystem.Application.UseCases.PayMealUseCase;
using Microsoft.Extensions.DependencyInjection;

namespace CanteenBillingSystem.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<ICreditClientAccountUseCase, CreditClientAccountUseCase>();
            services.AddScoped<IGetClientsUseCase, GetClientsUseCase>();
            services.AddScoped<IPayMealUseCase, PayMealUseCase>();

            return services;
        }
    }
}