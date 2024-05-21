namespace Template.WebApi.Configuration.Security;

internal static class CorsConfigurationExtensions
{
    static readonly string AllowedOrigins = "_applyInsuranceApi";

    internal static IServiceCollection AddCorsWithOrigins(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        var origins = configuration.GetSection("AllowedOrigins").Value!.Split("|");
        return services.AddCors(options =>
            options.AddPolicy(AllowedOrigins, policy =>
            {
                if (environment.IsDevelopment())
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                else
                    policy
                        .WithOrigins([.. origins])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            }));
    }

    internal static IApplicationBuilder UseCorsWithOrigins(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseCors(AllowedOrigins);
}
