namespace CanteenBillingSystem.API.Middlewares
{
    public record ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}