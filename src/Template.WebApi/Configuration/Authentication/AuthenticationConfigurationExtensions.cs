using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Template.WebApi.Configuration.Authentication;

internal static class AuthenticationConfigurationExtensions
{
    internal static IServiceCollection AddAuthenticationWithJwtBearer(this WebApplicationBuilder builder) =>
        builder.Configuration.AuthenticationDisabled()
            ? builder.Services
            : builder.Services
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    var authConfiguration = builder.Configuration
                        .GetSection(JwtBearerConfiguration.ConfigurationSectionPosition)
                        .Get<JwtBearerConfiguration>();

                    options.Authority = authConfiguration!.Authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidAudiences = authConfiguration.ValidAudiences,
                    };
                }).Services
                .AddAuthorizationBuilder()
                .SetDefaultPolicy(
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build())
                .Services;

    internal static IEndpointConventionBuilder ConfigureAuthorizatrion(this IEndpointConventionBuilder builder, IConfiguration configuration) =>
        configuration.AuthenticationDisabled()
            ? builder
            : builder.RequireAuthorization();

    internal static bool AuthenticationDisabled(this IConfiguration configuration)
    {
        var jwtBearerConfiguration = configuration
            .GetSection("Authentication:Schemes:Bearer")
            .Get<JwtBearerConfiguration>();

        return jwtBearerConfiguration!.Disabled;
    }
}
