namespace Template.WebApi.Configuration.Authentication;

public record JwtBearerConfiguration(bool Disabled, string? Authority, IEnumerable<string>? ValidAudiences)
{
    /// <summary>
    /// Position in configuration source. Eg. appsettings.json
    /// </summary>
    internal static readonly string ConfigurationSectionPosition = "Authentication:Schemes:Bearer";
}
