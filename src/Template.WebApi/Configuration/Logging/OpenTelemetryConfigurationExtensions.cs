using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Template.WebApi.Configuration.Logging;

internal static class OpenTelemetryConfigurationExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        // ApplicationName
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(builder.Environment.ApplicationName);

        builder.Logging
            .ClearProviders()
            .AddOpenTelemetry(options =>
            {
                options.IncludeScopes = true;
                options.IncludeFormattedMessage = true;
                options.ParseStateValues = true;
                options
                    .SetResourceBuilder(resourceBuilder)
                    .AddConsoleExporter();
            }).Services
            .AddOpenTelemetry()
            .WithTracing(tracing => tracing
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter())
            .WithMetrics(metrics => metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddConsoleExporter());

        return builder;
    }
}
