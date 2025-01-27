using Asp.Versioning;
using CanteenBillingSystem.Application.UseCases.CreditClientAccount;
using CanteenBillingSystem.Application.UseCases.GetClients;
using Microsoft.AspNetCore.Mvc;

namespace CanteenBillingSystem.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clients")]
    public class ClientController : ControllerBase
    {
        private readonly ICreditClientAccountUseCase _creditClientAccountUseCase;
        private readonly IGetClientsUseCase _getClientsUseCase;

        public ClientController(
            ICreditClientAccountUseCase creditClientAccountUseCase,
            IGetClientsUseCase getClientsUseCase)
        {
            _creditClientAccountUseCase = creditClientAccountUseCase;
            _getClientsUseCase = getClientsUseCase;
        }

        [HttpPost("{clientId}/credit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreditClientAccountResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreditAccount(Guid clientId, [FromBody] CreditAccountRequest request)
        {
            var accountBalance = await _creditClientAccountUseCase.ExecuteAsync(clientId, request.Amount);

            return Ok(new CreditClientAccountResponse(true, "Account credited successfully.", accountBalance));
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _getClientsUseCase.ExecuteAsync();
            return Ok(clients);
        }
    }
}