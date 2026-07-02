using System.Net;
using System.Text.Json;

namespace EmployeeApi.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode statusCode;
        string message;

        switch (ex)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = ex.Message;
                break;

            case BadHttpRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;

                // ⭐ TEMPORARY FOR DEBUGGING
                // Show full exception details so Azure reveals the real error
                message = ex.ToString();
                break;
        }

        var response = new
        {
            StatusCode = (int)statusCode,
            Message = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
