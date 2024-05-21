using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.WebApi.Configuration.Swagger;

internal static class SwaggerConfigurationExtensions
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.AddJwtBearer(configuration);
            });

    internal static IApplicationBuilder UseSwaggerAndUI(this WebApplication builder) =>
        builder
            .UseSwagger()
            .UseSwaggerUI(
                options =>
                {
                    var descriptions = builder.DescribeApiVersions();
                    // build a swagger endpoint for each discovered API version
                    foreach (var groupName in descriptions.Select(description => description.GroupName))
                    {
                        var url = $"/swagger/{groupName}/swagger.json";
                        var name = groupName.ToUpperInvariant();

                        options.SwaggerEndpoint(url, name);
                    }
                });
}