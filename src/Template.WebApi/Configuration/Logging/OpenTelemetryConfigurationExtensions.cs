using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Template.WebApi.Configuration.Logging;

internal static class OpenTelemetryConfigurationExtensions
{
    public static WebApplicationBuilder AddAndConfigureOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        // ApplicationName
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(configuration.GetSection("Logging:ApplicationName").Value!);

        builder.Logging
            .ClearProviders()
            .AddOpenTelemetry(options =>
            {
                options.IncludeScopes = true;
                options.IncludeFormattedMessage = true;
                options.ParseStateValues = true;
                options.SetResourceBuilder(resourceBuilder);

                if (builder.Environment.IsDevelopment())
                    options.AddConsoleExporter();
                else
                    options.AddAzureMonitorLogExporter(options => options.ConnectionString
                        = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"));
            }).Services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (!builder.Environment.IsDevelopment())
                    tracing.AddAzureMonitorTraceExporter(options => options.ConnectionString
                        = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"));
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                if (!builder.Environment.IsDevelopment())
                    metrics.AddAzureMonitorMetricExporter(options => options.ConnectionString
                       = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString"));
            });

        return builder;
    }
}
