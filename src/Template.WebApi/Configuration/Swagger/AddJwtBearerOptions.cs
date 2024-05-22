using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Template.WebApi.Configuration.Authentication;

namespace Template.WebApi.Configuration.Swagger;

internal static class AddJwtBearerOptions
{
    /// <summary>
    /// Adds possilbity to use JWT Bearer token when using the Swagger/OpenAPI UI.
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <returns></returns>
    internal static SwaggerGenOptions AddJwtBearer(
        this SwaggerGenOptions swaggerGenOptions,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (!configuration.AuthenticationEnabled())
        {
            return swaggerGenOptions;
        }
        swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        return swaggerGenOptions;
    }
}
