namespace Template.WebApi.Configuration.Authentication;

public class JwtBearerConfiguration
{
    public bool Enabled { get; set; }
    public string? Authority { get; set; }
    public IEnumerable<string>? ValidAudiences { get; set; }

    /// <summary>
    /// Position in configuration source. Eg. appsettings.json
    /// </summary>
    public static readonly string ConfigurationSectionPosition = "Authentication:Schemes:Bearer";
}
