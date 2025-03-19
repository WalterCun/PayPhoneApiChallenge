using System.Net;
using System.Text.Json;
using PayPhoneApiChallenge.Wrappers;
using PayPhoneApiChallenge.Exceptions;

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
            logger.LogError(ex, "Error procesando la solicitud");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Ocurrió un error inesperado.";

        // Mapear las excepciones a códigos HTTP y mensajes
        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case BadRequestException:
            case ArgumentException or InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            default:
                message = "Error interno en el servidor.";
                break;
        }

        var response = new ApiResponse<object>(
            success: false,
            message: message,
            data: null
        );
        
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(json);
    }
}
