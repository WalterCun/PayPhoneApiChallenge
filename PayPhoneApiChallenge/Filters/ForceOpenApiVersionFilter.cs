using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PayPhoneApiChallenge.Filters;

public class ForceOpenApiVersionFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Si el campo Version está vacío, lo forzamos a "3.0.1"
        if (string.IsNullOrWhiteSpace(swaggerDoc.Info.Version))
        {
            swaggerDoc.Info.Version = "3.0.1";
        }
    }
}