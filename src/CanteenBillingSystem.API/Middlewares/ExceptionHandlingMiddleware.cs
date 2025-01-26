using CanteenBillingSystem.Application.Exceptions;
using CanteenBillingSystem.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CanteenBillingSystem.API.Middlewares;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidAmountException ex)
        {
            _logger.LogWarning("Invalid amount exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ClientNotFoundException ex)
        {
            _logger.LogWarning("Client not found exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (InsufficientBalanceException ex)
        {
            _logger.LogWarning("Insufficient balance exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.PaymentRequired, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Success = false,
            Message = message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}