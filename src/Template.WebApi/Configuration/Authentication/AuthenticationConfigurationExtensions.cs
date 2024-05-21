using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Template.WebApi.Configuration.Authentication;

public static class AuthenticationConfigurationExtensions
{
    public static WebApplicationBuilder AddAuthenticationWithJwtBearer(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        if (!builder.Configuration.AuthenticationEnabled())
        {
            return builder;
        }
        builder.Services
            .AddAuthentication()
            .AddJwtBearer(opts =>
            {
                var authConfiguration = builder.Configuration.GetSection(JwtBearerConfiguration.ConfigurationSectionPosition).Get<JwtBearerConfiguration>();

                opts.Authority = authConfiguration!.Authority;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = authConfiguration.ValidAudiences,
                };
            });

        builder.Services
            .AddAuthorizationBuilder()
            .SetDefaultPolicy(
                new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

        return builder;
    }

    public static IEndpointConventionBuilder ConfigureAuthorizatrion(this IEndpointConventionBuilder handler, IConfiguration configuration)
    {
        if (configuration.AuthenticationEnabled())
            handler.RequireAuthorization();

        return handler;
    }

    public static bool AuthenticationEnabled(this IConfiguration configuration)
    {
        var authConfiguration = configuration.GetSection(JwtBearerConfiguration.ConfigurationSectionPosition).Get<JwtBearerConfiguration>();
        return authConfiguration != null && authConfiguration.Enabled;
    }
}
