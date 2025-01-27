namespace CanteenBillingSystem.API.Controllers.v1
{
    public record CreditClientAccountResponse(
     bool Success,
     string Message,
     decimal? NewBalance);
}