﻿using System.Net;
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
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string message = "Ocurrió un error inesperado.";
        string? errorCode = null;

        // Mapear las excepciones a códigos HTTP y mensajes
        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                errorCode = "NOT_FOUND";
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                errorCode = "BAD_REQUEST";
                break;

            case ArgumentException or InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                errorCode = "INVALID_ARGUMENT";
                break;

            default:
                // Aquí puedes loguear el stacktrace si quieres más detalle
                message = "Error interno en el servidor.";
                errorCode = "INTERNAL_SERVER_ERROR";
                break;
        }

        var response = new ApiResponse<object>(
            success: false,
            message: message,
            data: null
        );

        // Opcional: agregar errorCode al JSON si lo quieres mostrar
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(json);
    }
}
