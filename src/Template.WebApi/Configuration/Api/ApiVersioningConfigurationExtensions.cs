using Asp.Versioning;

namespace Template.WebApi;

internal static class ApiVersioningConfigurationExtensions
{
    internal static IServiceCollection AddApiVersioningAndExplorer(this IServiceCollection services) =>
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .Services;
}
