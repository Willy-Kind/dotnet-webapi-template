using System.Diagnostics;

namespace Template.WebApi.Configuration.Logging;

internal static class CorrelationHeaderPropagationMiddleware
{
    internal static IApplicationBuilder UseCorrelationHeaderPropagationMiddleware(this IApplicationBuilder builder)
    {
        builder.Use(async (context, next) =>
        {
            if (!context.Request.Headers.TryGetValue(TelemetryConstants.CORRELATION_ID_KEY, out var correlationId))
                correlationId = Guid.NewGuid().ToString();

            // Adds LfCorrelationId to Otel Activity
            Activity.Current?.SetTag(TelemetryConstants.CORRELATION_ID_KEY, correlationId.ToString());

            // Adds LfCorrelationId to ILogger
            var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("CorrelationIdMiddleware");
            using var scope = logger.BeginScope(new Dictionary<string, object> { { TelemetryConstants.CORRELATION_ID_KEY, correlationId.ToString() } });

            await next.Invoke(context);
        });

        return builder;
    }
}
