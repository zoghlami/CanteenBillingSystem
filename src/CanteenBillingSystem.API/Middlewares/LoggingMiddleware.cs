namespace CanteenBillingSystem.API.Middlewares;

using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(await FormatRequest(context));
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation(await FormatResponse(context, stopwatch.Elapsed));
        }
    }

    private async Task<string> FormatRequest(HttpContext context)
    {
        var request = context.Request;
        request.EnableBuffering();

        var body = string.Empty;
        if (request.Body.CanSeek)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            body = await reader.ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
        }

        return $"Incoming Request:\n" +
               $"  Method: {request.Method}\n" +
               $"  Path: {request.Path}\n" +
               $"  QueryString: {request.QueryString}\n" +
               $"  Body: {body}";
    }

    private Task<string> FormatResponse(HttpContext context, TimeSpan duration)
    {
        var response = context.Response;
        return Task.FromResult(
            $"Outgoing Response:\n" +
            $"  StatusCode: {response.StatusCode}\n" +
            $"  Duration: {duration.TotalMilliseconds} ms"
        );
    }
}