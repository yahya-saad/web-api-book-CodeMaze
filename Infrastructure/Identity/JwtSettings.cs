namespace Infrastructure.Identity;
internal class JwtSettings
{
    public const string Section = "JwtSettings";
    public string validIssuer { get; set; }
    public string validAudience { get; set; }
    public string SecretKey { get; set; }
    public int expires { get; set; }
}

