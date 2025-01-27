using Asp.Versioning;
using CanteenBillingSystem.API.Middlewares;
using CanteenBillingSystem.API.Swagger;
using CanteenBillingSystem.Application.UseCases.PayMealUseCase;
using CanteenBillingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace CanteenBillingSystem.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clients")]
    public class MealController : ControllerBase
    {
        private readonly IPayMealUseCase _payMealUseCase;

        public MealController(IPayMealUseCase payMealUseCase)
        {
            _payMealUseCase = payMealUseCase;
        }

        [HttpPost("{clientId}/pay-meal")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MealPaymentResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerRequestExample(typeof(List<MealItemRequest>), typeof(MealItemRequestExample))]
        public async Task<IActionResult> PayMeal(Guid clientId, [FromBody] List<MealItemRequest> request)
        {
            var response = await _payMealUseCase.ExecuteAsync(clientId, request);

            return Ok(response);
        }
    }
}