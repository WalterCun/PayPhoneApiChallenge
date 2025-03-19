using System.Net;
using System.Text.Json;

namespace PayPhoneApiChallenge.Milddlewares;


public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Logueamos el error para análisis
            logger.LogError(ex, "Error en el procesamiento de la solicitud");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Por defecto, usamos 500 Internal Server Error.
        var code = HttpStatusCode.InternalServerError;
        
        // if (exception is ArgumentException) code = HttpStatusCode.BadRequest;
        if (exception is ArgumentException or InvalidOperationException)
        {
            code = HttpStatusCode.BadRequest;
        }
        
        var result = JsonSerializer.Serialize(new { message = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}