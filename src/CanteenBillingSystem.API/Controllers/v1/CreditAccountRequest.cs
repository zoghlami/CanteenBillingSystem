using System.ComponentModel.DataAnnotations;

namespace CanteenBillingSystem.API.Controllers.v1
{
    public class CreditAccountRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}